import { useEffect, useState } from "react";
import { eventoService } from "../../services/eventPlusservice";
import type { Evento } from "../../types/api";

import "./Eventos.css";

import Header from "../../components/header/Header";
import Footer from "../../components/footer/Footer";
import CardEvento from "../../components/cardEvento/CardEvento";

function Eventos() {
  const [eventos, setEventos] = useState<Evento[]>([]);
  const [categoriaAtiva, setCategoriaAtiva] = useState<string>("Todos");
  const [carregando, setCarregando] = useState<boolean>(true);

  useEffect(() => {
    async function carregar() {
      try {
        setCarregando(true);
        const dados = await eventoService.listar();
        setEventos(dados);
      } catch (error) {
        console.error("Erro ao carregar eventos:", error);
      } finally {
        setCarregando(false);
      }
    }

    void carregar();
  }, []);

  // Extrai dinamicamente as categorias existentes dos eventos
  const categorias = [
    "Todos",
    ...Array.from(
      new Set(
        eventos
          .map((e) => e.idTipoEventoNavigation?.titulo)
          .filter((titulo): titulo is string => Boolean(titulo))
      )
    ),
  ];

  // Filtra os eventos exibidos com base na categoria selecionada
  const eventosFiltrados =
    categoriaAtiva === "Todos"
      ? eventos
      : eventos.filter(
          (e) => e.idTipoEventoNavigation?.titulo === categoriaAtiva
        );

  return (
    <>
      <Header />
      <main className="main-eventos">
        <div className="container-eventos">
          {/* Botões de Filtro Por Categoria */}
          <div className="filtros-container">
            {categorias.map((cat) => (
              <button
                key={cat}
                type="button"
                className={`filtro-btn ${categoriaAtiva === cat ? "ativo" : ""}`}
                onClick={() => setCategoriaAtiva(cat)}
              >
                {cat}
              </button>
            ))}
          </div>

          {/* Grade de Eventos */}
          {carregando ? (
            <p className="status-mensagem">Carregando catálogo...</p>
          ) : eventosFiltrados.length === 0 ? (
            <p className="status-mensagem">Nenhum evento encontrado.</p>
          ) : (
            <div className="eventos-grid">
              {eventosFiltrados.map((evento) => (
                <CardEvento
                  key={evento.idEvento}
                  id={evento.idEvento}
                  imagem={evento.imagemUrl}
                  categoria={evento.idTipoEventoNavigation?.titulo}
                  titulo={evento.nome}
                  descricao={evento.descricao}
                />
              ))}
            </div>
          )}
        </div>
      </main>
      <Footer />
    </>
  );
}

export default Eventos;