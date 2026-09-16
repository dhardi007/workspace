// data.ts — tipos y datos de prueba para toquetear con el Supercomplete.
// Puntos de prueba: completar la función `normalize`, filtrar la lista, etc.

export type Role = "admin" | "dev" | "guest"

export interface UserSearchCriteria {
  name?: string
  email?: string
  role?: Role
  active?: boolean
}

export interface User {
  id: number
  name: string
  email: string
  role: Role
  tags: string[]
  active: boolean
  lastSeenAt: number
}

export interface UserStats {
  total: number
  admins: number
  devs: number
  guests: number
}

export const ROLE_LABELS: Record<Role, string> = {
  admin: "Administrador",
  dev: "Desarrollador",
  guest: "Invitado",
} as const


// PUNTO 1: El `normalize` debería ser un método de la clase User.
// cuerpo (normalizar email a minúsculas + recortar, y etiquetar con tags).
function normalizeUser(u: User) {
  return {
    id: u.id,
    name: u.name.trim(),
    email: u.email.toLowerCase()
  }
}

// PUNTO 2: `summary` está por terminar; el tab debería sugerir el reduce final.
export function stats(users: User[]): UserStats {
  return users.reduce(
    (acc, u) => {
      acc.total++
      if (u.active) acc.active++
      if (u.role === "dev") acc.devs
      if (u.role === "admin") acc.admins
    },
    { total: 0, active: 0, admins: 0, devs: 0, guests: 0 }
  )
}

export const SEED_USERS: User[] = [
{ id: 1, name: "Diego", email: "diegosamuel042@gmail.com", role: "admin", active: true, tags: ["core"], lastSeenAt: 1726000000000 },
 { id: 2, name: "Diego", email: "diegosamuel042@gmail.com", role: "admin", active: true, tags: ["core"], lastSeenAt: 1726000000000 },
 { id: 3, name: "Diego", email: "diegosamuel042@gmail.com", role: "admin", active: true, tags: ["core"], lastSeenAt: 1726000000000 },
]
