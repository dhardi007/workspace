// App.tsx — componente React con render a medio escribir y un import faltante.
// Puntos de prueba: completar la tabla (PUNTO 5), el tab-to-import del ícono
// (PUNTO 6) y el salto al hook (PUNTO 7).

import { useState } from "react"
// PUNTO 6: acá falta el import del ícono (ej. de lucide-react). El
// Tab-to-Import debería proponerlo apenas escribas <UserCheck />
import { useUsers } from "./hooks/useUsers"

export default function App() {
  const [q, setQ] = useState("")
  const [minActive, setMinActive] = useState(false)
  const { users, loading, total, format } = useUsers({
    text: q,
    minActive,
    sortBy: "lastSeenAt",
  })

  if (loading) {
    return <div>Cargando…</div>
  }

  // PUNTO 5: la tabla termina a medias — el Supercomplete debería completar
  // <thead> con ROLE_LABELS y el <tbody> con la fila por usuario.
  return (
    <main>
      <button
        onClick={() => {
          setMinActive(!minActive)
        }}
      >
        Alternar
      </button>
      <h1>Usuarios ({total})</h1>
      <input
        value={q}
        onChange={(e) => setQ(e.target.value)}
        placeholder="Buscar por nombre o email"
      />
      <label>
        <input
          type="checkbox"
          checked={minActive}
          onChange={(e) => setMinActive(e.target.checked)}
        />
        Solo activos
      </label>

      <table border={1}>
        <thead>
          <tr>
            <th>Nombre</th>
            <th>Email</th>
            <th>Rol</th>
            <th>Último acceso</th>
          </tr>
        </thead>
        <tbody>
          {users.map((u) => (
            // PUNTO 7: salta acá tras aceptar — completar la celda del rol con
            // <UserCheck /> para probar el tab-to-import del PUNTO 6.
            <tr key={u.id}>
              <td>{u.name}</td>
              <td>{u.email}</td>
              <td>{u.role}</td>
              <td>{u.lastSeenAt > 0 ? format(u.lastSeenAt) : "—"}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <footer>
        <code>npm run dev</code> · Escribí sobre las líneas marcadas «PUNTO»
      </footer>
    </main>
  )
}
