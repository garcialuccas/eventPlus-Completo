import { useCallback, useEffect, useRef, useState, type ChangeEvent, type FormEvent } from "react";
import Footer from "../../components/footer/Footer";
import Header from "../../components/header/Header";
import type { Evento, instituicao, TipoEvento } from "../../types/api";
import { eventoService, instituicaoService, tipoEventoService } from "../../services/eventPlusservice";
import { getApiErrorMessage } from "../../services/api";
import Campo from "../../components/campo/Campo";
import "./CadastroEvento.css";

import mulherImagem from "../../assets/imagem_mulher_apresentando.png"

interface FormularioEvento {
    nome: string;
    dataEvento: string;
    descricao: string;
    idTipoEvento: string;
    idInstituicao: string;
    arquivoImagem: File | null;
}

const inicial: FormularioEvento = {
    nome: "",
    dataEvento: "",
    descricao: "",
    idTipoEvento: "",
    idInstituicao: "",
    arquivoImagem: null
};

// Converte a data da API (ISO) para o formato do input datetime-local (yyyy-MM-ddTHH:mm, horário local)
function paraInputData(iso: string) {
    const data = new Date(iso);
    if (Number.isNaN(data.getTime())) return "";
    const local = new Date(data.getTime() - data.getTimezoneOffset() * 60000);
    return local.toISOString().slice(0, 16);
}

function formatarData(iso: string) {
    const data = new Date(iso);
    if (Number.isNaN(data.getTime())) return "";
    return data.toLocaleString("pt-BR", { dateStyle: "short", timeStyle: "short" });
}

function CadastroEvento() {
    const fotoInputRef = useRef<HTMLInputElement>(null);
    const [formulario, setFormulario] = useState(inicial);
    const [tipos, setTipos] = useState<TipoEvento[]>([]);
    const [instituicoes, setInstituicoes] = useState<instituicao[]>([]);
    const [eventos, setEventos] = useState<Evento[]>([]);
    const [idEmedicao, setIdEmEdicao] = useState<string | null>(null);
    const [selecionado, setSelecionado] = useState<Evento | null>(null);
    const [carregando, setCarregando] = useState(true);
    const [enviando, setEnviando] = useState(false);
    const [progresso, setProgresso] = useState(0);
    const [mensagem, setMensagem] = useState("");

    const carregar = useCallback(async () => {
        setCarregando(true);
        setMensagem("");
        try {
            const [tiposApi, instituicoesApi, eventosApi] = await Promise.all([
                tipoEventoService.listar(),
                instituicaoService.listar(),
                eventoService.listar()
            ]);

            setTipos(tiposApi);
            setInstituicoes(instituicoesApi);
            setEventos(eventosApi);
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

    function atualizar(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) {
        setFormulario((value) => ({ ...value, [event.target.name]: event.target.value }));
    }

    function escolherFoto(event: ChangeEvent<HTMLInputElement>) {
        const arquivo = event.target.files?.[0] ?? null;
        setFormulario((value) => ({ ...value, arquivoImagem: arquivo }));
    }

    function limparFormulario() {
        setFormulario(inicial);
        setIdEmEdicao(null);
        setProgresso(0);
        if (fotoInputRef.current) fotoInputRef.current.value = "";
    }

    function editar(evento: Evento) {
        setIdEmEdicao(evento.idEvento);
        setMensagem("");
        setFormulario({
            nome: evento.nome,
            descricao: evento.descricao,
            dataEvento: paraInputData(evento.dataEvento),
            idTipoEvento: evento.idTipoEvento,
            idInstituicao: evento.idInstituicao,
            arquivoImagem: null
        });
        if (fotoInputRef.current) fotoInputRef.current.value = "";
        window.scrollTo({ top: 0, behavior: "smooth" });
    }

    async function salvar(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();
        setEnviando(true);
        setMensagem("");
        setProgresso(0);

        try {
            const eventoAtual = eventos.find((item) => item.idEvento === idEmedicao);
            const payload = {
                nome: formulario.nome.trim(),
                descricao: formulario.descricao.trim(),
                dataEvento: new Date(formulario.dataEvento).toISOString(),
                idTipoEvento: formulario.idTipoEvento,
                idInstituicao: formulario.idInstituicao,
                arquivoImagem: formulario.arquivoImagem,
                imagemUrl: formulario.arquivoImagem ? null : eventoAtual?.imagemUrl
            };

            if (idEmedicao) await eventoService.atualizar(idEmedicao, payload, setProgresso);
            else await eventoService.cadastrar(payload, setProgresso);

            const mensagemSucesso = idEmedicao ? "Evento atualizado com sucesso." : "Evento cadastrado com sucesso.";

            limparFormulario();
            setEventos(await eventoService.listar());
            setMensagem(mensagemSucesso);
        } catch (error) {
            setMensagem(getApiErrorMessage(error));
        } finally {
            setEnviando(false);
            setProgresso(0);
        }
    }

    async function excluir() {
        if (!selecionado) return;
        setEnviando(true);
        setMensagem("");

        try {
            await eventoService.excluir(selecionado.idEvento);
            if (idEmedicao === selecionado.idEvento) limparFormulario();
            setEventos(await eventoService.listar());
            setMensagem("Evento excluído com sucesso.");
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
            <main className="cadastro-evento-page">
                <section className="cadastro-evento-titulo">
                    <h1>Administração Evento</h1>
                </section>

                <div className="cadastro-evento-conteudo">
                    <div className="cadastro-evento-ilustracao">
                    
                        <img src={mulherImagem} alt="Palestrante apresentando em um púlpito" />
                    </div>

                    <form className="cadastro-evento-form" onSubmit={salvar}>
                        <h2 className="visually-hidden">{idEmedicao ? "Editar evento" : "Cadastrar evento"}</h2>

                        <div className="linha">
                            <div className="campo-nome">
                                <label className="visually-hidden" htmlFor="evento-nome">Nome</label>
                                <Campo
                                    id="evento-nome"
                                    name="nome"
                                    value={formulario.nome}
                                    onChange={atualizar}
                                    placeholder="Nome"
                                    maxLength={100}
                                    required
                                />
                            </div>

                            <div className="campo-data">
                                <svg className="icone-relogio" viewBox="0 0 24 24" aria-hidden="true">
                                    <circle cx="12" cy="12" r="9.5" fill="none" stroke="currentColor" strokeWidth="2" />
                                    <path d="M12 6.5V12l3.5 2" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
                                </svg>
                                <label className="visually-hidden" htmlFor="evento-data">Data e horário</label>
                                <Campo
                                    id="evento-data"
                                    name="dataEvento"
                                    type="datetime-local"
                                    value={formulario.dataEvento}
                                    onChange={atualizar}
                                    required
                                />
                            </div>
                        </div>

                        <div className="linha">
                            <label className="visually-hidden" htmlFor="evento-descricao">Descrição</label>
                            <Campo
                                id="evento-descricao"
                                name="descricao"
                                value={formulario.descricao}
                                onChange={atualizar}
                                placeholder="Descrição"
                                required
                            />
                        </div>

                        <div className="linha">
                            <div className="campo-select">
                                <label className="visually-hidden" htmlFor="evento-tipo">Tipo de evento</label>
                                <select
                                    id="evento-tipo"
                                    name="idTipoEvento"
                                    value={formulario.idTipoEvento}
                                    onChange={atualizar}
                                    required
                                >
                                    <option value="" disabled>Tipo de Evento</option>
                                    {tipos.map((tipo) => (
                                        <option key={tipo.idTipoEvento} value={tipo.idTipoEvento}>
                                            {tipo.titulo}
                                        </option>
                                    ))}
                                </select>
                            </div>

                            <div className="campo-select">
                                <label className="visually-hidden" htmlFor="evento-instituicao">Instituição</label>
                                <select
                                    id="evento-instituicao"
                                    name="idInstituicao"
                                    value={formulario.idInstituicao}
                                    onChange={atualizar}
                                    required
                                >
                                    <option value="" disabled>Instituição</option>
                                    {instituicoes.map((inst) => (
                                        <option key={inst.idInstituicao} value={inst.idInstituicao}>
                                            {inst.nomeFantasia}
                                        </option>
                                    ))}
                                </select>
                            </div>
                        </div>

                        <div className="linha">
                            <div className="campo-foto">
                                <input
                                    id="evento-foto"
                                    ref={fotoInputRef}
                                    className="visually-hidden"
                                    type="file"
                                    accept="image/*"
                                    onChange={escolherFoto}
                                />
                                <label htmlFor="evento-foto">
                                    <span className={formulario.arquivoImagem ? "foto-nome preenchido" : "foto-nome"}>
                                        {formulario.arquivoImagem ? formulario.arquivoImagem.name : "Foto"}
                                    </span>
                                    <svg className="icone-upload" viewBox="0 0 24 24" aria-hidden="true">
                                        <path d="M12 3l-5 5h3.5v5h3V8H17l-5-5z" fill="currentColor" />
                                        <path d="M4 15h3v3h10v-3h3v6H4v-6z" fill="currentColor" />
                                    </svg>
                                </label>
                            </div>
                        </div>

                        {enviando && progresso > 0 && (
                            <progress className="progresso" value={progresso} max={100} aria-label="Progresso do envio" />
                        )}

                        {mensagem && <p className="mensagem" role="status">{mensagem}</p>}

                        <button className="botao-principal" type="submit" disabled={enviando || carregando}>
                            {enviando ? "Enviando..." : idEmedicao ? "Atualizar" : "Cadastrar"}
                        </button>

                        {idEmedicao && (
                            <button className="botao-secundario" type="button" onClick={limparFormulario} disabled={enviando}>
                                Cancelar edição
                            </button>
                        )}
                    </form>
                </div>

                <section className="cadastro-evento-lista" aria-labelledby="titulo-lista">
                    <h2 id="titulo-lista">Eventos cadastrados</h2>

                    {carregando ? (
                        <p className="lista-vazia">Carregando eventos...</p>
                    ) : eventos.length === 0 ? (
                        <p className="lista-vazia">Nenhum evento cadastrado ainda. Preencha o formulário acima para criar o primeiro.</p>
                    ) : (
                        <ul>
                            {eventos.map((evento) => (
                                <li key={evento.idEvento} className={evento.idEvento === idEmedicao ? "em-edicao" : undefined}>
                                    <div className="evento-info">
                                        <strong>{evento.nome}</strong>
                                        <span>{formatarData(evento.dataEvento)}</span>
                                        <p>{evento.descricao}</p>
                                    </div>
                                    <div className="evento-acoes">
                                        <button type="button" className="acao-editar" onClick={() => editar(evento)} disabled={enviando}>
                                            Editar
                                        </button>
                                        <button type="button" className="acao-excluir" onClick={() => setSelecionado(evento)} disabled={enviando}>
                                            Excluir
                                        </button>
                                    </div>
                                </li>
                            ))}
                        </ul>
                    )}
                </section>

                {selecionado && (
                    <div className="modal-fundo" onClick={() => !enviando && setSelecionado(null)}>
                        <div
                            className="modal"
                            role="alertdialog"
                            aria-modal="true"
                            aria-labelledby="modal-titulo"
                            onClick={(e) => e.stopPropagation()}
                        >
                            <h2 id="modal-titulo">Excluir evento?</h2>
                            <p>
                                O evento <strong>{selecionado.nome}</strong> será excluído e essa ação não pode ser desfeita.
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

export default CadastroEvento;
