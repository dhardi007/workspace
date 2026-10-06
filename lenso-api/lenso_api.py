#!/usr/bin/env python3
"""Cliente no-oficial de lenso.ai reverse image search.

Descifrado del protocolo (ver README abajo):
  - POST /api/search  con body {image:{id}, effects, selection, ...}
  - token tkn viaja en window.lenso.tkn; tokenEncrypted = Il(tkn)
  - los resultados traen type:"LOCKED" hasta llamar POST /api/unlock-result (pago)
"""
import hashlib, json, sys, urllib.error, urllib.request, http.cookiejar

BASE = 'https://lenso.ai'
UA = 'Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/131.0.0.0 Safari/537.36'
CATEGORIES = ['people', 'duplicates', 'places', 'related', 'similar']


def encrypt_token(tkn: str) -> str:
    """Portado exacto de Il() del bundle common-DGc-cBrhP4.js."""
    n = bytearray(b ^ 170 for b in tkn.encode())
    for i in range(len(n)):
        n[i] = (~n[i] ^ i) & 0xFF
    s = bytes(n).decode('latin-1')[2:-2]
    md5 = hashlib.md5(s.encode('latin-1', 'replace')).hexdigest()
    chunks = [md5[i:i + 4] for i in range(0, len(md5), 4)]
    return ''.join(reversed([c for i, c in enumerate(chunks) if i % 2 == 1]))


def fetch_tkn():
    r = urllib.request.urlopen(urllib.request.Request(f'{BASE}/en', headers={'User-Agent': UA}))
    html = r.read().decode()
    start = html.index('window.lenso = ') + len('window.lenso = ')
    return json.loads(html[start:html.index('\n', start)])['tkn']


class Lenso:
    def __init__(self):
        self.jar = http.cookiejar.CookieJar()
        self.op = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(self.jar))
        self.hdr = {'Content-Type': 'application/json', 'Accept': 'application/json',
                    'Origin': BASE, 'Referer': f'{BASE}/en', 'User-Agent': UA}
        self.tkn = fetch_tkn()

    def _post(self, path, body):
        req = urllib.request.Request(BASE + path, data=json.dumps(body).encode(),
                                     headers=self.hdr, method='POST')
        try:
            return json.loads(self.op.open(req).read())
        except urllib.error.HTTPError as e:
            return {'_status': e.code, '_error': e.read().decode()[:300]}

    def search(self, image_id, category='duplicates', page=1, selection=None, sort='', seed=0):
        """image_id = el segmento de la URL: /en/results/<image_id>?..."""
        sel = selection or {'top': 0, 'left': 0, 'right': 1, 'bottom': 1}
        return self._post('/api/search', {
            'image': {'id': image_id},
            'effects': {'rotation': 0},
            'selection': sel,
            'domain': '', 'text': '', 'page': page,
            'type': category, 'sort': sort.upper(), 'seed': seed,
            'facial_search_consent': 0,
        })

    def unlock(self, proxy_url, result_hash, category):
        """Requiere plan de pago (401 en free tier)."""
        return self._post('/api/unlock-result',
                          {'proxyUrl': proxy_url, 'hash': result_hash, 'category': category})

    def upload(self, image_b64, fmt='image/jpeg'):
        return self._post('/api/upload', {
            'image': image_b64, 'facial_search_consent': False,
            'token': self.tkn, 'tokenEncrypted': encrypt_token(self.tkn),
        })


if __name__ == '__main__':
    img = sys.argv[1]
    cats = sys.argv[2].split(',') if len(sys.argv) > 2 else CATEGORIES
    cli = Lenso()
    print('tkn len:', len(cli.tkn), '| tokenEncrypted:', encrypt_token(cli.tkn), '\n')
    for c in cats:
        d = cli.search(img, c)
        if '_status' in d:
            print(f'{c:11} ERROR {d["_status"]} {d["_error"]}')
            continue
        res = d.get('results', [])
        locked = sum(1 for r in res if r.get('type') == 'LOCKED')
        print(f'{c:11} category={d.get("category"):11} n={len(res):3} locked={locked:3} '
              f'pages={d.get("availablePages")} searchHash={d.get("searchHash")}')
