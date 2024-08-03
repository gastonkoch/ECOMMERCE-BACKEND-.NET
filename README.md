# ECOMMERCE: Aplicacion BACKEND para tienda online.

Este es el back-end del Ecommerce, se debe levantar la solucion para poder consumir las API desde el Front. Como por ejemplo lo que implemento en este repositorio https://github.com/gastonkoch/ECOMMERCE-FRONTEND-REACT

Para este proyecto se utilizaron estos conceptos de la programación:

🟢 POO.

🟢 ORM: Entity Framework.

🟢 Base de datos con SQL Lite.

🟢 APIs.

🟢 Tipo de dato generico.

🟢 Clean Code Architecture.

🟢 Inyección de dependencias.

🟢 Envio de Mails utilizando paquetes de terceros.

🟢 Patrón repositorio.

🟢 Autenticacion con Json Web Token.

🟢 Manejo de errores con excepciones.

🟢 Uso de DTOs.

🟢 Lenguaje unificado de modelado (UML).

🟢 Políticas de permisos.



# REGLAS DE NEGOCIO

➖ El usuario tiene acceso a ingresar al sistema o registrarse y poder crear una orden de compra.

➖ Una orden de compra posee un cliente, un vendedor y una lista de productos que pueden ser uno o más.

➖ El vendedor puede crear, visualizar, modificar y realizar bajas lógicas en los productos.

➖ El admin puede realizar bajas físicas de las órdenes de compra.

➖ Al momento de realizar una compra o cuando se modifica un estado, se enviará un mail tanto al cliente como al vendedor informándolos.

➖ Una orden debe poseer un producto o más para ser creada.

➖ El cliente y el vendedor podrán modificar datos de la orden, como métodos de pago, cantidad de producto, entre otras funciones.

➖ El admin podrá ver un resumen de órdenes asignadas a clientes y asignadas a vendedores.

➖ El cliente podrá, desde el endpoint de order, validar que el producto que desea agregar a la orden posea stock.

➖ El vendedor es el encargado de cambiar los estados de la orden.

➖ Todos los roles, incluyendo los usuarios sin sesión, podrán ver nuestros productos habilitados.

➖ El vendedor es el encargado de la alta, modificación, cambio de stock del producto y, junto al admin, responsables de la baja lógica.

➖ El admin es el único que podrá realizar bajas físicas del producto.

➖ El admin es el rol que más funciones posee con respecto a la manipulación de los usuarios. Puede ver todos los usuarios estén disponibles o no, buscar tanto por nombre como por ID, hacer bajas lógicas y físicas.

➖ El rol cliente y vendedor podrán modificar su usuario.

#Tecnologías Utilizadas
➖ Lenguaje: C#
➖ Framework: .NET Core
➖ Base de Datos: SQL Server
➖ ORM: Entity Framework Core
➖ Autenticación: Json Web Token (JWT)
➖ Inyección de Dependencias: Microsoft.Extensions.DependencyInjection


♦️ El repositorio cuenta con pocos commits debido a que el proyecto se llevo a cabo en otro repositorio privado 
