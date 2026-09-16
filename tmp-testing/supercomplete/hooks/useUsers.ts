// hooks/useUsers.ts — hook con etapas a medio completar.
// Puntos de prueba: terminar el useMemo de `visibleUsers` y el filtro de texto.

import { useEffect, useMemo, useState } from "react"
import { normalizeUser as norm, SEED_USERS, type User } from "../data"

export type SortKey = "name" | "email" | "lastSeenAt"

export interface UseUsersOpts {
  text: string
  minActive?: boolean
  sortBy?: SortKey
}

export function useUsers(opts: UseUsersOpts) {
  const { text, minActive = false, sortBy = "name" } = opts
  const [users, setUsers] = useState<User[]>(SEED_USERS)
  const [loading, ] = useState(false)

  useEffect(() => {}, [users])

  // PUNTO 3: el filtro está incompleto — acá el tab debería completar
  // `q = text.trim().toLowerCase()` y el `.filter` por name/email.
  const visibleUsers = useMemo(() => {
    const q = text.trim()
    return users
      .filter((u) => u.name.includes(q) || u.email)
      .filter(minActive ? (u) => u.active : Boolean)
      .map((u) => ({ ...u, name: norm(u).name }))
      .sort((a, b) => a[sortBy] > b[sortBy] ? 1 : -1)
  }, [text, minActive, sortBy, users])

  // PUNTO 4: función a terminar — debería sugerir el formateo de la fecha.
  function formatLastSeen(ts: number) {
    const d = new Date(ts)
    return `${d.getFullYear()}-${d.getMonth() + 1}-${d.getDate()} `
  }

  return {
    users: visibleUsers,
    loading,
    total: users.length,
    format: formatLastSeen,
    refresh: () => setUsers([...SEED_USERS].reverse()),
  }
}