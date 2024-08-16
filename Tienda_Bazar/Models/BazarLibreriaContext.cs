using Microsoft.EntityFrameworkCore;
using Tienda_Bazar.Models;

public class BazarLibreriaContext : DbContext
{
    public BazarLibreriaContext(DbContextOptions<BazarLibreriaContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<EstadoUsuario> EstadoUsuarios { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<ImagenProducto> ImagenesProductos { get; set; }
    public DbSet<CarritoCompra> CarritoCompras { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<DetallePedido> DetallesPedidos { get; set; }
    public DbSet<Resena> Resenas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>()
            .Property(p => p.Estado)
            .HasDefaultValue(false);  // Define un valor por defecto para Estado, aunque será actualizado por el trigger

        modelBuilder.Entity<Producto>()
            .HasMany(p => p.ImagenesProductos)
            .WithOne(i => i.Producto)
            .HasForeignKey(i => i.CodigoProducto);

        modelBuilder.Entity<Producto>()
            .HasMany(p => p.CarritoCompras)
            .WithOne(c => c.Producto)
            .HasForeignKey(c => c.CodigoProducto);

        modelBuilder.Entity<Producto>()
            .HasMany(p => p.DetallesPedidos)
            .WithOne(d => d.Producto)
            .HasForeignKey(d => d.CodigoProducto);

        modelBuilder.Entity<Producto>()
            .HasMany(p => p.Resenas)
            .WithOne(r => r.Producto)
            .HasForeignKey(r => r.CodigoProducto);

        modelBuilder.Entity<CarritoCompra>()
            .HasOne(c => c.Usuario)
            .WithMany(u => u.CarritoCompras)
            .HasForeignKey(c => c.CodigoUsuario);

        modelBuilder.Entity<CarritoCompra>()
            .HasOne(c => c.Producto)
            .WithMany(p => p.CarritoCompras)
            .HasForeignKey(c => c.CodigoProducto);

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Usuario)
            .WithMany(u => u.Pedidos)
            .HasForeignKey(p => p.CodigoUsuario);

        modelBuilder.Entity<DetallePedido>()
            .HasOne(d => d.Pedido)
            .WithMany(p => p.DetallesPedidos)
            .HasForeignKey(d => d.CodigoPedido);

        modelBuilder.Entity<DetallePedido>()
            .HasOne(d => d.Producto)
            .WithMany(p => p.DetallesPedidos)
            .HasForeignKey(d => d.CodigoProducto);

        modelBuilder.Entity<Resena>()
            .HasOne(r => r.Producto)
            .WithMany(p => p.Resenas)
            .HasForeignKey(r => r.CodigoProducto);

        modelBuilder.Entity<Resena>()
            .HasOne(r => r.Usuario)
            .WithMany(u => u.Resenas)
            .HasForeignKey(r => r.CodigoUsuario);

        modelBuilder.Entity<Usuario>()
            .Property(u => u.CodigoUsuario)
            .ValueGeneratedOnAdd(); // Asegura que la base de datos genere automáticamente este valor

        modelBuilder.Entity<Usuario>()
            .Property(u => u.UltimaConexion)
            .HasDefaultValueSql("GETDATE()"); // Establece un valor por defecto generado por la base de datos

        modelBuilder.Entity<EstadoUsuario>().HasData(
                new EstadoUsuario { EstadoUsuarioId = 1, Nombre = "Activo" },
                new EstadoUsuario { EstadoUsuarioId = 2, Nombre = "Inactivo" }
            );

        modelBuilder.Entity<Rol>().HasData(
            new Rol { RolId = 1, Nombre = "Administrador" },
            new Rol { RolId = 2, Nombre = "Usuario" }
        );


        base.OnModelCreating(modelBuilder);
    }
}
