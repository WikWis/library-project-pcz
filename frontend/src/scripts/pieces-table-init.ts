import type {PieceDTO} from "../dto/piece/piece-dto.ts";


function renderPieceTableRow(pieceDTO: PieceDTO): string {
    return `<tr>
                    <td>${pieceDTO.titleName}</td>
                    <td>${pieceDTO.status}</td>
                    <td>${pieceDTO.state}</td>
                    <td><a href="/egzemplarz/${pieceDTO.pieceId}">Historia wypożyczenia</a></td>
            </tr>`
}

export function renderPiecesTable(pieceDTOS: PieceDTO[]): string {
    const tableRows: string = pieceDTOS.map(renderPieceTableRow).join("\n");

    return `
         <table class="table table-striped table-bordered">
            <thead>
            <tr>
                <th scope="col">Tytuł</th>
                <th scope="col">Status</th>
                <th scope="col">Stan</th>
                <th scope="col">Linki</th>
            </tr>
            </thead>
            <tbody>
                ${tableRows}
            </tbody>
         </table>
        `
}


