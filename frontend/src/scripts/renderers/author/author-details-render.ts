import type {TitleLookupDTO} from "../../../dto/title/title-lookup-dto.ts";

function renderTitle(titleLookupDTO: TitleLookupDTO): string {
    return `
        <li  class="list-group-item d-flex justify-content-between align-items-center">
        <div>
            <strong>${titleLookupDTO.name}</strong>
            <br>
            <small class="text-muted">
                ID: ${titleLookupDTO.titleId}
            </small>
        </div>
            
        <span class="badge bg-secondary">
            ${titleLookupDTO.dateOfPublishing}
        </span>
    </li>
    `
}

export function renderTitles(titleLookUpDTOs: TitleLookupDTO[]): string {
    if (titleLookUpDTOs.length === 0) {
        return `
            <div class="alert alert-light border text-muted mb-0">
                Autor nie ma przypisanych żadnych tytułów.
            </div>
        `
    }

    let renderedTitles = titleLookUpDTOs.map(renderTitle).join("\n")
    return `<ul class="list-group">` + renderedTitles + `</ul>`
}