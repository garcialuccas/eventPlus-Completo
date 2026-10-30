import "./ModalConfirmacao.css";

interface ModalConfirmacaoProps {
  show: boolean;
  titulo: string;
  mensagem: string;
  textoConfirmar?: string;
  textoCancelar?: string;
  onClose: () => void;
  onConfirm: () => void;
}

function ModalConfirmacao({
  show,
  titulo,
  mensagem,
  textoConfirmar = "Confirmar",
  textoCancelar = "Cancelar",
  onClose,
  onConfirm,
}: ModalConfirmacaoProps) {
  if (!show) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-container" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h3>{titulo}</h3>
          <button type="button" className="modal-close-btn" onClick={onClose}>
            &times;
          </button>
        </div>
        
        <div className="modal-body">
          <p>{mensagem}</p>
        </div>

        <div className="modal-footer">
          <button type="button" className="btn-manter" onClick={onClose}>
            {textoCancelar}
          </button>
          <button type="button" className="btn-cancelar-modal" onClick={onConfirm}>
            {textoConfirmar}
          </button>
        </div>
      </div>
    </div>
  );
}

export default ModalConfirmacao;