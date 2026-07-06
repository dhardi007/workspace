import type { APIRoute } from "astro";

export const prerender = false;

export const POST: APIRoute = async ({ request }) => {
  try {
    const body = await request.json();
    const { name, email, message, website } = body;

    if (website) {
      return new Response(JSON.stringify({ error: "Bot detected" }), { status: 400 });
    }

    if (!name?.trim() || !email?.trim() || !message?.trim()) {
      return new Response(JSON.stringify({ error: "Todos los campos son requeridos" }), { status: 400 });
    }

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      return new Response(JSON.stringify({ error: "Email inválido" }), { status: 400 });
    }

    const RESEND_API_KEY = import.meta.env.RESEND_API_KEY;

    if (RESEND_API_KEY) {
      const { Resend } = await import("resend");
      const resend = new Resend(RESEND_API_KEY);
      await resend.emails.send({
        from: "Contacto dhardi.dev <onboarding@resend.dev>",
        to: "diegosamuel042@gmail.com",
        replyTo: email,
        subject: `Nuevo mensaje de ${name} — dhardi.dev`,
        html: `<p><strong>Nombre:</strong> ${name}</p><p><strong>Email:</strong> ${email}</p><p><strong>Mensaje:</strong></p><p>${message}</p>`,
      });
    }

    return new Response(JSON.stringify({ ok: true }), { status: 200 });
  } catch {
    return new Response(JSON.stringify({ error: "Error interno" }), { status: 500 });
  }
};
