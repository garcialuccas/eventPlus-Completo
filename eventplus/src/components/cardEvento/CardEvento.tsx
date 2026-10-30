import { Link } from "react-router-dom";
import "./CardEvento.css";

interface CardEventoProps {
  id: string | number;
  titulo: string;
  imagem?: string;
  descricao?: string;
  categoria?: string;
}

function CardEvento({ id, titulo, imagem, descricao, categoria }: CardEventoProps) {
  return (
    <article className="card-evento">
      <div className="card-evento-imagem-container">
        <img src={imagem} alt={titulo} />
      </div>
      
      <div className="card-evento-conteudo">
        {categoria && <span className="card-evento-categoria">{categoria}</span>}
        <h3 className="card-evento-titulo">{titulo}</h3>
        <p className="card-evento-descricao">{descricao}</p>
        
        <Link to={`/eventos/${id}`} className="card-evento-link">
          Ver evento
        </Link>
      </div>
    </article>
  );
}

export default CardEvento;