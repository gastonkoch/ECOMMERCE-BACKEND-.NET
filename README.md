# ECOMMERSE-BACKEND
Este es el back-end del Ecommerse, se debe levantar la solucion para poder consumir las API desde el Front en React que se encuentra en otro repositorio.

Este proyecto esta hecho para implementar los siguientes conceptos:

✅ Clean Code Architecture.

✅ ORM: Entity Framework.

✅ Base de datos con SQL.

✅ APIs.

✅ Inyección de dependencias.

✅ Patrón repositorio.

✅ Tipo de dato generico.

✅ Autenticacion con Json Web Token.

✅ Envio de Mails utilizando paquetes de terceros.

✅ Lenguaje unificado de modelado (UML).

✅ Políticas de permisos.

✅ Manejo de errores con excepciones.

✅ Uso de DTOs.

✅ POO.

# Reglas de negocio

• El usuario tiene acceso a ingresar al sistema o registrarse y poder crear una orden de compra.

• Una orden de compra posee un cliente, un vendedor y una lista de productos que pueden ser uno o más.

• El vendedor puede crear, visualizar, modificar y realizar bajas lógicas en los productos.

• El admin puede realizar bajas físicas de las órdenes de compra.

• Al momento de realizar una compra o cuando se modifica un estado, se enviará un mail tanto al cliente como al vendedor informándolos.

• Una orden debe poseer un producto o más para ser creada.

• El cliente y el vendedor podrán modificar datos de la orden, como métodos de pago, cantidad de producto, entre otras funciones.

• El admin podrá ver un resumen de órdenes asignadas a clientes y asignadas a vendedores.

• El cliente podrá, desde el endpoint de order, validar que el producto que desea agregar a la orden posea stock.

• El vendedor es el encargado de cambiar los estados de la orden.

• Todos los roles, incluyendo los usuarios sin sesión, podrán ver nuestros productos habilitados.

• El vendedor es el encargado de la alta, modificación, cambio de stock del producto y, junto al admin, responsables de la baja lógica.

• El admin es el único que podrá realizar bajas físicas del producto.

• El admin es el rol que más funciones posee con respecto a la manipulación de los usuarios. Puede ver todos los usuarios estén disponibles o no, buscar tanto por nombre como por ID, hacer bajas lógicas y físicas.

• El rol cliente y vendedor podrán modificar su usuario.


>El repo tiene pocos commits porque subi lo que tenia en un repositorio privado.
