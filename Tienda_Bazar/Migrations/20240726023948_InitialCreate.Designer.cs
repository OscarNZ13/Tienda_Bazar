﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Tienda_Bazar.Migrations
{
    [DbContext(typeof(BazarLibreriaContext))]
    [Migration("20240726023948_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Tienda_Bazar.Models.CarritoCompra", b =>
                {
                    b.Property<int>("CodigoCarrito")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoCarrito"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("CodigoProducto")
                        .HasColumnType("int");

                    b.Property<int>("CodigoUsuario")
                        .HasColumnType("int");

                    b.HasKey("CodigoCarrito");

                    b.HasIndex("CodigoProducto");

                    b.HasIndex("CodigoUsuario");

                    b.ToTable("CarritoCompras");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.DetallePedido", b =>
                {
                    b.Property<int>("CodigoDetalle")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoDetalle"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("CodigoPedido")
                        .HasColumnType("int");

                    b.Property<int>("CodigoProducto")
                        .HasColumnType("int");

                    b.Property<decimal>("PrecioUnitario")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("CodigoDetalle");

                    b.HasIndex("CodigoPedido");

                    b.HasIndex("CodigoProducto");

                    b.ToTable("DetallesPedidos");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.EstadoUsuario", b =>
                {
                    b.Property<int>("EstadoUsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EstadoUsuarioId"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("EstadoUsuarioId");

                    b.ToTable("EstadoUsuarios");

                    b.HasData(
                        new
                        {
                            EstadoUsuarioId = 1,
                            Nombre = "Activo"
                        },
                        new
                        {
                            EstadoUsuarioId = 2,
                            Nombre = "Inactivo"
                        });
                });

            modelBuilder.Entity("Tienda_Bazar.Models.ImagenProducto", b =>
                {
                    b.Property<int>("CodigoImagen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoImagen"));

                    b.Property<int>("CodigoProducto")
                        .HasColumnType("int");

                    b.Property<byte[]>("Imagen")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("CodigoImagen");

                    b.HasIndex("CodigoProducto");

                    b.ToTable("ImagenesProductos");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Pedido", b =>
                {
                    b.Property<int>("CodigoPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoPedido"));

                    b.Property<int>("CodigoUsuario")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaPedido")
                        .HasColumnType("datetime2");

                    b.HasKey("CodigoPedido");

                    b.HasIndex("CodigoUsuario");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Producto", b =>
                {
                    b.Property<int>("CodigoProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoProducto"));

                    b.Property<int>("DisponibilidadInventario")
                        .HasColumnType("int");

                    b.Property<bool>("Estado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("NombreProducto")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("CodigoProducto");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Resena", b =>
                {
                    b.Property<int>("CodigoResena")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoResena"));

                    b.Property<int>("CodigoProducto")
                        .HasColumnType("int");

                    b.Property<int>("CodigoUsuario")
                        .HasColumnType("int");

                    b.Property<string>("DescipcionResena")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("FechaResena")
                        .HasColumnType("datetime2");

                    b.HasKey("CodigoResena");

                    b.HasIndex("CodigoProducto");

                    b.HasIndex("CodigoUsuario");

                    b.ToTable("Resenas");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Rol", b =>
                {
                    b.Property<int>("RolId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RolId"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RolId");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            RolId = 1,
                            Nombre = "Administrador"
                        },
                        new
                        {
                            RolId = 2,
                            Nombre = "Usuario"
                        });
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Usuario", b =>
                {
                    b.Property<int>("CodigoUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoUsuario"));

                    b.Property<string>("Contrasena")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("EstadoUsuarioId")
                        .HasColumnType("int");

                    b.Property<string>("NombreUsuario")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RolId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UltimaConexion")
                        .HasColumnType("datetime2");

                    b.HasKey("CodigoUsuario");

                    b.HasIndex("EstadoUsuarioId");

                    b.HasIndex("RolId");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.CarritoCompra", b =>
                {
                    b.HasOne("Tienda_Bazar.Models.Producto", "Producto")
                        .WithMany("CarritoCompras")
                        .HasForeignKey("CodigoProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tienda_Bazar.Models.Usuario", "Usuario")
                        .WithMany("CarritoCompras")
                        .HasForeignKey("CodigoUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.DetallePedido", b =>
                {
                    b.HasOne("Tienda_Bazar.Models.Pedido", "Pedido")
                        .WithMany("DetallesPedidos")
                        .HasForeignKey("CodigoPedido")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tienda_Bazar.Models.Producto", "Producto")
                        .WithMany("DetallesPedidos")
                        .HasForeignKey("CodigoProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.ImagenProducto", b =>
                {
                    b.HasOne("Tienda_Bazar.Models.Producto", "Producto")
                        .WithMany("ImagenesProductos")
                        .HasForeignKey("CodigoProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Pedido", b =>
                {
                    b.HasOne("Tienda_Bazar.Models.Usuario", "Usuario")
                        .WithMany("Pedidos")
                        .HasForeignKey("CodigoUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Resena", b =>
                {
                    b.HasOne("Tienda_Bazar.Models.Producto", "Producto")
                        .WithMany("Resenas")
                        .HasForeignKey("CodigoProducto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tienda_Bazar.Models.Usuario", "Usuario")
                        .WithMany("Resenas")
                        .HasForeignKey("CodigoUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Usuario", b =>
                {
                    b.HasOne("Tienda_Bazar.Models.EstadoUsuario", "EstadoUsuario")
                        .WithMany("Usuarios")
                        .HasForeignKey("EstadoUsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Tienda_Bazar.Models.Rol", "Rol")
                        .WithMany("Usuarios")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EstadoUsuario");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.EstadoUsuario", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Pedido", b =>
                {
                    b.Navigation("DetallesPedidos");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Producto", b =>
                {
                    b.Navigation("CarritoCompras");

                    b.Navigation("DetallesPedidos");

                    b.Navigation("ImagenesProductos");

                    b.Navigation("Resenas");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Rol", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("Tienda_Bazar.Models.Usuario", b =>
                {
                    b.Navigation("CarritoCompras");

                    b.Navigation("Pedidos");

                    b.Navigation("Resenas");
                });
#pragma warning restore 612, 618
        }
    }
}
