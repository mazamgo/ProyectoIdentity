﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoIdentity.Models;

namespace ProyectoIdentity.Datos
{
    //IdentityDbContext: contiene una serie de Herramientas, se crean tabla de usuarios, roles, todo lo necesita para la autenticacion
    //DbContext:

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        //Agregar los modelos necesarios
        public DbSet<AppUsuario> AppUsuarios { get; set; }

    }
}
