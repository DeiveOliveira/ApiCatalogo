﻿using APICatalogo.Context;
using APICatalogo.Interface;
using APICatalogo.Models;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context) 
        { 
            _context = context;
        }


        public Categoria Create(Categoria categoria)
        {
            if(categoria == null) throw new ArgumentNullException(nameof(categoria));

            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return categoria;
        }

        public Categoria Delete(int id)
        {
            var categoria = _context.Categorias.Find(id);
            
            if(categoria is null) throw new ArgumentNullException(nameof(categoria));
            
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            return categoria;
        }

        public Categoria GetCategoria(int id)
        {
            return _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
        }

        public IEnumerable<Categoria> GetCategorias()
        {
            return _context.Categorias.ToList();
        }

        public Categoria Update(Categoria categoria)
        {
            if(categoria == null) throw new ArgumentNullException(nameof(categoria));

            _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return categoria;
        }
    }
}
