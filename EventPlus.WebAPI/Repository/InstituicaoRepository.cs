using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.DTO;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EventPlus.WebAPI.Repository
{
    public class InstituicaoRepository : IInstituicao
    {
        private readonly EventContext _context;

        public InstituicaoRepository(EventContext context)
        {
            _context = context;
        }


        public async Task Atualizar(Guid id, Instituicao instituicao)
        {
            var instituicaoEncontrada = await _context.Instituicao.FindAsync(id);

            if (instituicaoEncontrada != null) 
            {
                instituicaoEncontrada.Cnpj = instituicao.Cnpj;
                instituicaoEncontrada.NomeFantasia = instituicao.NomeFantasia;
                instituicaoEncontrada.Endereco = instituicao.Endereco;

                 _context.Instituicao.Update(instituicaoEncontrada);
                 await _context.SaveChangesAsync();
            }

        }

        public async Task<Instituicao?> BuscarPorId(Guid id)
        {
            return await _context.Instituicao.FirstOrDefaultAsync(x => x.Idinstituicao == id);
        }

        public async Task Cadastrar(Instituicao instituicao)
        {
            await _context.Instituicao.AddAsync(instituicao);
            await _context.SaveChangesAsync();
        }

        public async Task Deletar(Guid id)
        {
            var InstituicaoEncontrada = await _context.Instituicao.FindAsync(id);

            if (InstituicaoEncontrada != null) 
            {
                _context.Instituicao.Remove(InstituicaoEncontrada);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Instituicao>> Listar()
        {
            return await _context.Instituicao.AsNoTracking().ToListAsync();
        }
    }
}
