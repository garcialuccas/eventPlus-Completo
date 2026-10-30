import { useCallback, useEffect, useState, type ChangeEvent, type FormEvent } from "react";
import Footer from "../../components/footer/Footer";
import Header from "../../components/header/Header";
import type { TipoEvento } from "../../types/api";
import { tipoEventoService } from "../../services/eventPlusservice";
import { getApiErrorMessage } from "../../services/api";
import Campo from "../../components/campo/Campo";
import "../../components/CssComponente/CadastroTipo.css";

import imagemTipoEventoBanner from "../../assets/imagemBannerTipoEvento.svg"

function CadastroTipoEvento() {
    const [titulo, setTitulo] = useState("");
    const [tipos, setTipos] = useState<TipoEvento[]>([]);
    const [idEmEdicao, setIdEmEdicao] = useState<string | null>(null);
    const [selecionado, setSelecionado] = useState<TipoEvento | null>(null);
    const [carregando, setCarregando] = useState(true);
    const [enviando, setEnviando] = useState(false);
    const [mensagem, setMensagem] = useState("");

    const carregar = useCallback(async () => {
        setCarregando(true);
        setMensagem("");
        try {
            setTipos(await tipoEventoService.listar());
        } catch (error) {
            setMensagem(getApiErrorMessage(error));
        } finally {
            setCarregando(false);
        }
    }, []);

    useEffect(() => {
        const timeout = window.setTimeout(() => void carregar(), 0);
        return () => window.clearTimeout(timeout);
    }, [carregar]);

    function atualizar(event: ChangeEvent<HTMLInputElement>) {
        setTitulo(event.target.value);
    }

    function limparFormulario() {
        setTitulo("");
        setIdEmEdicao(null);
    }

    function editar(tipo: TipoEvento) {
        setIdEmEdicao(tipo.idTipoEvento);
        setTitulo(tipo.titulo);
        setMensagem("");
        window.scrollTo({ top: 0, behavior: "smooth" });
    }

    async function salvar(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();
        setEnviando(true);
        setMensagem("");

        try {
            const payload = { titulo: titulo.trim() };

            if (idEmEdicao) await tipoEventoService.atualizar(idEmEdicao, payload.titulo);
            else await tipoEventoService.cadastrar(payload.titulo);

            const mensagemSucesso = idEmEdicao
                ? "Tipo de evento atualizado com sucesso."
                : "Tipo de evento cadastrado com sucesso.";

            limparFormulario();
            setTipos(await tipoEventoService.listar());
            setMensagem(mensagemSucesso);
        } catch (error) {
            setMensagem(getApiErrorMessage(error));
        } finally {
            setEnviando(false);
        }
    }

    async function excluir() {
        if (!selecionado) return;
        setEnviando(true);
        setMensagem("");

        try {
            await tipoEventoService.excluir(selecionado.idTipoEvento);
            if (idEmEdicao === selecionado.idTipoEvento) limparFormulario();
            setTipos(await tipoEventoService.listar());
            setMensagem("Tipo de evento excluído com sucesso.");
            setSelecionado(null);
        } catch (error) {
            setMensagem(getApiErrorMessage(error));
            setSelecionado(null);
        } finally {
            setEnviando(false);
        }
    }

    return (
        <>
            <Header />
            <main className="cadastro-tipo-page">
                <section className="tipo-topo">
                    <h1 className="titulo-pagina">Administração Tipo de Eventos</h1>

                    <div className="tipo-conteudo">
                        <div className="tipo-ilustracao">
                           
                            <img src={imagemTipoEventoBanner} alt="Duas pessoas cadastrando itens em uma tela de computador" />
                        </div>

                        <form className="tipo-form" onSubmit={salvar}>
                            <h2 className="visually-hidden">{idEmEdicao ? "Editar tipo de evento" : "Cadastrar tipo de evento"}</h2>

                            <div>
                                <label className="visually-hidden" htmlFor="tipo-evento-titulo">Título</label>
                                <Campo
                                    id="tipo-evento-titulo"
                                    name="titulo"
                                    value={titulo}
                                    onChange={atualizar}
                                    placeholder="Título"
                                    maxLength={100}
                                    autoComplete="off"
                                    required
                                />
                            </div>

                            {mensagem && <p className="mensagem" role="status">{mensagem}</p>}

                            <button className="botao-principal" type="submit" disabled={enviando || carregando}>
                                {enviando ? "Enviando..." : idEmEdicao ? "Atualizar" : "Cadastrar"}
                            </button>

                            {idEmEdicao && (
                                <button className="botao-secundario" type="button" onClick={limparFormulario} disabled={enviando}>
                                    Cancelar edição
                                </button>
                            )}
                        </form>
                    </div>
                </section>

                <section className="tipo-lista" aria-labelledby="titulo-lista-tipo-evento">
                    <h2 id="titulo-lista-tipo-evento" className="titulo-pagina">Lista Tipo de Eventos</h2>

                    {carregando ? (
                        <p className="lista-vazia">Carregando tipos de evento...</p>
                    ) : tipos.length === 0 ? (
                        <p className="lista-vazia">Nenhum tipo de evento cadastrado ainda. Preencha o título acima para criar o primeiro.</p>
                    ) : (
                        <div className="tabela-rolagem">
                            <table>
                                <thead>
                                    <tr>
                                        <th scope="col" className="coluna-titulo">Título</th>
                                        <th scope="col" className="coluna-acao">Editar</th>
                                        <th scope="col" className="coluna-acao">Deletar</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {tipos.map((tipo) => (
                                        <tr key={tipo.idTipoEvento}>
                                            <td className="coluna-titulo">{tipo.titulo}</td>
                                            <td className="coluna-acao">
                                                <button
                                                    type="button"
                                                    className="icone-botao"
                                                    aria-label={`Editar ${tipo.titulo}`}
                                                    onClick={() => editar(tipo)}
                                                    disabled={enviando}
                                                >
                                                    <svg viewBox="0 0 24 24" aria-hidden="true">
                                                        <path
                                                            fill="currentColor"
                                                            d="M3 17.25V21h3.75L17.8 9.94l-3.75-3.75L3 17.25zM20.7 7.04a1 1 0 0 0 0-1.41l-2.34-2.34a1 1 0 0 0-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z"
                                                        />
                                                    </svg>
                                                </button>
                                            </td>
                                            <td className="coluna-acao">
                                                <button
                                                    type="button"
                                                    className="icone-botao"
                                                    aria-label={`Deletar ${tipo.titulo}`}
                                                    onClick={() => setSelecionado(tipo)}
                                                    disabled={enviando}
                                                >
                                                    <svg viewBox="0 0 24 24" aria-hidden="true">
                                                        <path
                                                            fill="currentColor"
                                                            d="M6 19a2 2 0 0 0 2 2h8a2 2 0 0 0 2-2V8H6v11zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z"
                                                        />
                                                    </svg>
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </section>

                {selecionado && (
                    <div className="modal-fundo" onClick={() => !enviando && setSelecionado(null)}>
                        <div
                            className="modal"
                            role="alertdialog"
                            aria-modal="true"
                            aria-labelledby="modal-titulo-tipo-evento"
                            onClick={(e) => e.stopPropagation()}
                        >
                            <h2 id="modal-titulo-tipo-evento">Excluir tipo de evento?</h2>
                            <p>
                                O tipo <strong>{selecionado.titulo}</strong> será excluído e essa ação não pode ser desfeita.
                            </p>
                            <div className="modal-acoes">
                                <button type="button" className="botao-secundario" onClick={() => setSelecionado(null)} disabled={enviando}>
                                    Cancelar
                                </button>
                                <button type="button" className="botao-principal" onClick={excluir} disabled={enviando}>
                                    {enviando ? "Excluindo..." : "Excluir"}
                                </button>
                            </div>
                        </div>
                    </div>
                )}
            </main>
            <Footer />
        </>
    );
}

export default CadastroTipoEvento;
