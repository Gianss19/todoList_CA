UPDATE usuarios
SET "rol" = 'Administrador',
    "FechaActualizacion" = NOW()
WHERE "Correo" = 'giancarlo@example.com';
Select * from usuarios;
81e02389-368f-4f34-8ec7-c377b7295133