import type {PublisherLookupDTO} from "../dto/publisher/publisher-lookup-dto.ts";


function renderPublisherTableRow(publisherLookupDTO: PublisherLookupDTO): string {
    return `<tr>
                <td>${publisherLookupDTO.name}</td>
                <td><a href="/wydawca/detale?id=${publisherLookupDTO.publisherId}">Lista książek</a></td>
            </tr>`
}

export function renderPublisherTable(publisherLookupDTOS: PublisherLookupDTO[]): string {
    const tableRows: string = publisherLookupDTOS.map(renderPublisherTableRow).join("\n");

    return `
         <table class="table table-striped table-bordered">
            <thead>
            <tr>
                <th scope="col">Nazwa wydawcy</th>
                <th scope="col">Lista książek</th>
            </tr>
            </thead>
            <tbody>
                ${tableRows}
            </tbody>
         </table>
        `
}


