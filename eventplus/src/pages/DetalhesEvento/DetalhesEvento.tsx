import { useNavigate, useParams } from "react-router-dom";
import { useCallback, useEffect, useState, type FormEvent } from "react";
import type { Presenca, Evento, Comentario, MensagemModeracao } from "../../types/api";

import imagemPadrao from "../../assets/banner-1.png";
import relogio from "../../assets/icons/relogio.svg";
import predio from "../../assets/icons/predio.svg";
import filtro from "../../assets/icons/filtro.svg";
import enviar from "../../assets/icons/enviar.svg";

import Header from "../../components/header/Header";
import Footer from "../../components/footer/Footer";
import Botao from "../../components/botao/Botao";

import "./DetalhesEvento.css";
import { comentarioService, eventoService, presencaService } from "../../services/eventPlusservice";
import { useAuth } from "../../context/auth";
import { getApiErrorMessage } from "../../services/api";
import ModalConfirmacao from "../../components/modalConfirmacao/ModalConfirmacao";
import Campo from "../../components/campo/Campo";

function formatarData(value?: string) {
  if (!value) return "Data não informada";
  const data = new Date(value);
  return Number.isNaN(data.getDate())
    ? "Data não informada"
    : new Intl.DateTimeFormat("pt-BR", { day: "2-digit", month: "long" }).format(data);
}

function formatarHora(value?: string) {
  if (!value) return "";
  const data = new Date(value);
  return Number.isNaN(data.getTime())
    ? ""
    : new Intl.DateTimeFormat("pt-BR", { hour: "2-digit", minute: "2-digit" }).format(data);
}

function DetalhesEvento() {
  const { id = "" } = useParams();
  const navigate = useNavigate();

  const { autenticado, usuario } = useAuth();
  const [evento, setEvento] = useState<Evento | null>(null);
  const [comentarios, setComentarios] = useState<Comentario[]>([]);
  const [secaoComentarios, setSecaoComentarios] = useState<boolean>(false);
  const [presenca, setPresenca] = useState<Presenca | null>(null);
  const [presencaCarregada, setPresencaCarregada] = useState("");
  const [confirmandoCancelamento, setConfirmandoCancelamento] = useState(false);
  const [enviandoInscricao, setEnviandoInscricao] = useState(false);
  const [mensagemInscricao, setMensagemInscricao] = useState("");
  const [, setCarregando] = useState(false);
  const [novoComentario, setNovoComentario] = useState("");
  const [mensagemComentario, setMensagemComentario] = useState("");
  const [enviandoComentario, setEnviandoComentario] = useState(false);
  const [, setErro] = useState("");

  const carregar = useCallback(async () => {
    if (!id) return;
    setCarregando(true);
    setErro("");
    try {
      const [eventoAtual, comentariosAtuais] = await Promise.all([
        eventoService.buscar(id),
        comentarioService.listarPorEvento(id),
      ]);
      setEvento(eventoAtual);
      setComentarios(comentariosAtuais);
    } catch (error) {
      setErro(getApiErrorMessage(error));
    } finally {
      setCarregando(false);
    }
  }, [id]);

  useEffect(() => {
    const timeout = window.setTimeout(() => void carregar(), 0);
    return () => window.clearTimeout(timeout);
  }, [carregar]);

  useEffect(() => {
    if (!usuario || !id) return;
    const chave = `${usuario.idUsuario}:${id}`;
    let ativo = true;

    presencaService
      .listarPorUsuario(usuario.idUsuario)
      .then((items) => {
        if (ativo) setPresenca(items.find((item) => item.idEvento === id) ?? null);
      })
      .catch(() => {
        if (ativo) setPresenca(null);
      })
      .finally(() => {
        if (ativo) setPresencaCarregada(chave);
      });
  }, [id, usuario]);

  function exigirLogin() {
    if (autenticado) return false;
    navigate("/login", { state: { from: location.pathname } });
    return true;
  }

  async function inscrever() {
    if (exigirLogin() || !usuario || !evento || presenca) return;
    setEnviandoInscricao(true);
    setMensagemInscricao("");

    try {
      const novaPresenca = await presencaService.cadastrar({
        situacao: true,
        idEvento: evento?.idEvento || "",
        idUsuario: usuario?.idUsuario || "",
      });
      setPresenca(novaPresenca);
      setMensagemInscricao("Inscrição realizada com sucesso.");
    } catch (error) {
      setMensagemInscricao(getApiErrorMessage(error));
    } finally {
      setEnviandoInscricao(false);
    }
  }

  async function cancelarInscricao() {
    if (!presenca || enviandoInscricao) return;
    setEnviandoInscricao(true);
    setMensagemInscricao("");

    try {
      await presencaService.excluir(presenca.idPresenca);
      setPresenca(null);
      setMensagemInscricao("Inscrição cancelada com sucesso.");
    } catch (error) {
      setMensagemInscricao(getApiErrorMessage(error));
    } finally {
      setConfirmandoCancelamento(false);
      setEnviandoInscricao(false);
    }
  }

  async function comentar(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (exigirLogin() || !usuario || !novoComentario.trim()) return;
    setEnviandoComentario(true);
    setMensagemComentario("");

    try {
      const response = await comentarioService.cadastrar({
        descricao: novoComentario.trim(),
        idEvento: evento?.idEvento || "",
        idUsuario: usuario.idUsuario,
      });
      setNovoComentario("");
      if ("mensagem" in response) {
        setMensagemComentario((response as MensagemModeracao).mensagem);
      } else {
        setComentarios((items) => [...items, response]);
        setMensagemComentario("Comentário publicado.");
      }
    } catch (error) {
      setMensagemComentario(getApiErrorMessage(error));
    } finally {
      setEnviandoComentario(false);
    }
  }

  const chavePresenca = usuario && id ? `${usuario.idUsuario}:${id}` : "";
  const checandoPresenca = Boolean(chavePresenca && presencaCarregada !== chavePresenca);

  const dataFormatada = formatarData(evento?.dataEvento);
  const horaFormatada = formatarHora(evento?.dataEvento);

  return (
    <>
      <Header />
      <main className="main-detalhes-evento">
        {/* Banner do Evento */}
        <img
          className="detalhe-banner"
          src={evento?.imagemUrl || imagemPadrao}
          alt={`Banner do evento ${evento?.nome || ""}`}
        />

        {/* Título Principal */}
        <h1 className="detalhe-titulo">{evento?.nome || "Carregando..."}</h1>

        {/* Grade Principal (Duas Colunas) */}
        <div className="detalhe-conteudo-grid">
          {/* Coluna Esquerda: Abas e Descrição / Comentários */}
          <div className="detalhe-coluna-esquerda">
            <div className="detalhe-abas">
              <button
                type="button"
                className={`detalhe-aba-btn ${!secaoComentarios ? "ativa" : ""}`}
                onClick={() => setSecaoComentarios(false)}
              >
                Sobre o evento
              </button>
              <button
                type="button"
                className={`detalhe-aba-btn ${secaoComentarios ? "ativa" : ""}`}
                onClick={() => setSecaoComentarios(true)}
              >
                Comentários
              </button>
            </div>

            {secaoComentarios ? (
              <div className="detalhe-evento-comentarios" role="tabpanel">
                <form className="detalhe-comentario-form" onSubmit={comentar}>
                  <label className="visually-hidden" htmlFor="novo-comentario">
                    Comentar
                  </label>
                  <Campo
                    id="novo-comentario"
                    value={novoComentario}
                    maxLength={200}
                    placeholder="Escreva um comentário..."
                    onChange={(event) => setNovoComentario(event.target.value)}
                  />
                  <button
                    type="submit"
                    className="detalhe-comentario-submit"
                    aria-label="Enviar comentário"
                    disabled={enviandoComentario}
                  >
                    <img src={enviar} alt="Ícone de enviar" />
                  </button>
                </form>

                {mensagemComentario ? (
                  <p className="detalhe-evento-mensagem" role="status">
                    {mensagemComentario}
                  </p>
                ) : null}

                {comentarios.length === 0 ? (
                  <p className="sem-comentarios">Nenhum comentário publicado</p>
                ) : (
                  comentarios.map((comentario) => (
                    <article key={comentario.idComentario} className="detalhe-evento-comentario">
                      <div>
                        <strong>
                          {comentario.idUsuarioNavigation?.nome || "Usuário"}
                        </strong>{" "}
                        <time>
                          {new Date(comentario.dataComentario).toLocaleString("pt-BR", {
                            day: "2-digit",
                            month: "2-digit",
                            hour: "2-digit",
                            minute: "2-digit",
                          })}
                        </time>
                        <p>{comentario.descricao}</p>
                      </div>
                    </article>
                  ))
                )}
              </div>
            ) : (
              <p className="detalhe-descricao">{evento?.descricao}</p>
            )}
          </div>

          {/* Coluna Direita: Informações Principais */}
          <aside className="detalhe-coluna-direita">
            <h2>INFORMAÇÕES PRINCIPAIS</h2>
            <hr />

            <div className="detalhe-info-item">
              <img src={relogio} alt="Ícone relógio" />
              <span>
                {dataFormatada} {horaFormatada ? `às ${horaFormatada}` : ""}
              </span>
            </div>

            <div className="detalhe-info-item">
              <img src={predio} alt="Ícone prédio" />
              <span>
                {evento?.idInstituicaoNavigation?.nomeFantasia ||
                  "Instituição não informada"}
              </span>
            </div>

            <div className="detalhe-info-item">
              <img src={filtro} alt="Ícone tipo de evento" />
              <span>
                {evento?.idTipoEventoNavigation?.titulo || "Categoria geral"}
              </span>
            </div>

            <Botao
              className={presenca ? "detalhe-evento-cancelar" : ""}
              texto={
                checandoPresenca
                  ? "Verificando inscrição..."
                  : presenca
                  ? "Cancelar Inscrição"
                  : enviandoInscricao
                  ? "Inscrevendo..."
                  : "Se-inscrever"
              }
              type="button"
              disabled={checandoPresenca || enviandoInscricao}
              onClick={() =>
                presenca ? setConfirmandoCancelamento(true) : void inscrever()
              }
            />

            {mensagemInscricao ? (
              <p className="detalhe-evento-mensagem" role="status">
                {mensagemInscricao}
              </p>
            ) : null}
          </aside>
        </div>

     
        <ModalConfirmacao
          show={confirmandoCancelamento}
          titulo="Cancelar inscrição"
          mensagem={`Deseja cancelar sua inscrição em "${evento?.nome || "este evento"}?"`}
          textoConfirmar={enviandoInscricao ? "Cancelando..." : "Cancelar Inscrição"}
          textoCancelar="Manter inscrição"
          onClose={() => {
            if (!enviandoInscricao) setConfirmandoCancelamento(false);
          }}
          onConfirm={() => void cancelarInscricao()}
        />
      </main>
      <Footer />
    </>
  );
}

export default DetalhesEvento;