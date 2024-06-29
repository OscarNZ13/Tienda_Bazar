USE BazarLibreriaDB;
GO

ALTER TABLE Usuario
ADD CONSTRAINT DF_UltimaConexion DEFAULT GETDATE() FOR UltimaConexion;
GO

ALTER TABLE Pedidos
ADD CONSTRAINT DF_FechaPedido DEFAULT GETDATE() FOR FechaPedido;
GO

ALTER TABLE Resenas
ADD CONSTRAINT DF_FechaResena DEFAULT GETDATE() FOR FechaResena;
GO

CREATE TRIGGER Trigger_ActualizarEstadoProducto
ON Productos
AFTER INSERT, UPDATE
AS
BEGIN
    -- Actualiza el estado de los productos dependiendo de la disponibilidad de inventario
    UPDATE p
    SET p.Estado = CASE 
                    WHEN i.DisponibilidadInventario > 0 THEN 1
                    ELSE 0
                 END
    FROM Productos p
    INNER JOIN Inserted i ON p.CodigoProducto = i.CodigoProducto;
END;
GO

-- Pueden descomentar esto para probar el trigger --

---- Insertar un nuevo producto
--INSERT INTO Productos (NombreProducto, Precio, DisponibilidadInventario)
--VALUES ('Nuevo Producto', 29.99, 10);

---- Verificar que el estado se ha actualizado automáticamente
--SELECT * FROM Productos WHERE CodigoProducto = 1;

---- Actualizar la disponibilidad del producto
--UPDATE Productos
--SET DisponibilidadInventario = 0
--WHERE CodigoProducto = 1;

---- Verificar que el estado se ha actualizado automáticamente
--SELECT * FROM Productos WHERE CodigoProducto = 1;