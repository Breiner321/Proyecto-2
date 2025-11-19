INSTRUCCIONES PARA AGREGAR LOS LOGOS:

1. Coloca tus archivos de imagen del logo en esta carpeta:
   
   - logo.png: Para la página de Login
     Ruta completa: Web/MvcSample/wwwroot/images/logo.png
   
   - logo2.png: Para los perfiles (Estudiante, Coordinador, Administrador)
     Ruta completa: Web/MvcSample/wwwroot/images/logo2.png
   
   (Formatos soportados: PNG, JPG, SVG)

2. Los logos se mostrarán automáticamente:
   - logo.png: Página de Login (Views/Auth/Login.cshtml)
   - logo2.png: Perfil Estudiante (Views/Shared/_StudentLayout.cshtml)
   - logo2.png: Perfil Coordinador (Views/Shared/_CoordinatorLayout.cshtml)
   - logo2.png: Perfil Administrador (Views/Shared/_AdminLayout.cshtml)

3. Si los logos no existen, se mostrará el texto por defecto automáticamente.

4. Si tus logos tienen nombres diferentes, edita los archivos mencionados y cambia los nombres de archivo.

¡Listo! Solo coloca tus imágenes en esta carpeta y se mostrarán automáticamente.

