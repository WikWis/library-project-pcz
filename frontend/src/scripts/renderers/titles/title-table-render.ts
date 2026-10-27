import type {TitleLookupDTO} from "../../../dto/title/title-lookup-dto.ts";


function renderTitleTableRow(titleLookupDTO: TitleLookupDTO): string {
    return `<tr>
                    <td>${titleLookupDTO.name}</td>
                    <td>${titleLookupDTO.isbn}</td>
                    <td>${titleLookupDTO.genres.join(", ").replace("\,$", "")}</td>
                    <td><a href="/tytul/detale?id=${titleLookupDTO.titleId}">Dowiedz się więcej</a></td>
                </tr>`
}

export function renderTitleTable(titleLookupDTO: TitleLookupDTO[]): string {
    const tableRows: string = titleLookupDTO.map(renderTitleTableRow).join("\n");

    return `
         <table class="table table-striped table-bordered">
            <thead>
            <tr>
                <th scope="col">Tytuł</th>
                <th scope="col">ISBN</th>
                <th scope="col">Gatunki</th>
                <th scope="col">Dowiedz się więcej</th>
            </tr>
            </thead>
            <tbody>
                ${tableRows}
            </tbody>
         </table>
        `
}


