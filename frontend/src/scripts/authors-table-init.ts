import type {AuthorLookupDTO} from "../dto/author/author-lookup-dto.ts";


function renderAuthorTableRow(authorLookupDTO: AuthorLookupDTO): string {
    return `<tr>
                    <td>${authorLookupDTO.name} ${authorLookupDTO.surname}</td>
                    <td><a href="/autorzy/${authorLookupDTO.authorId}">Czytaj więcej</a></td>
                </tr>`
}

export function renderAuthorTable(authorLookupDTOS: AuthorLookupDTO[]): string {
    const tableRows: string = authorLookupDTOS.map(renderAuthorTableRow).join("\n");

    return `
         <table class="table table-striped table-bordered">
            <thead>
            <tr>
                <th scope="col">Imię Nazwisko</th>
                <th scope="col">Książki</th>
            </tr>
            </thead>
            <tbody>
                ${tableRows}
            </tbody>
         </table>
        `
}


