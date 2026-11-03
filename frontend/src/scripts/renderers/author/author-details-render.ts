import type {AuthorDTO} from "../../../dto/author/author-dto.ts";
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


export function renderAuthorDetails(authorDto: AuthorDTO): string {
    return `
        <div class="container-fluid mt-4 px-4">
            <div class="card shadow-sm w-100">
                <div class="card-header bg-primary text-white">
                    <h4 class="mb-0">Szczegóły autora</h4>
                </div>

                <div class="card-body p-4">
                    <div class="row mb-3">
                        <div class="col-md-4">
                            <strong>ID autora:</strong>
                            <span>${authorDto.authorId}</span>
                        </div>

                        <div class="col-md-4">
                            <strong>Imię:</strong>
                            <span>${authorDto.name}</span>
                        </div>

                        <div class="col-md-4">
                            <strong>Nazwisko:</strong>
                            <span>${authorDto.surname}</span>
                        </div>
                    </div>

                    <hr>

                    <div class="mb-3">
                        <div class="d-flex justify-content-between align-items-center mb-3">
                            <strong>Opublikowane tytuły:</strong>
                            <span class="badge bg-primary">
                                ${authorDto.titles.length}
                            </span>
                        </div>

                        <div id="titles-list"></div>
                    
                    </div>

                    <div class="mb-3">
                        <div class="col d-flex justify-content-between">
                            <button type="button" class="btn btn-primary" id=previous-button disabled>Previous Page</button>
                            <button type="button" class="btn btn-primary" id=next-button>Next Page</button>
                        </div>
                    </div>
                    
                </div>
            </div>
        </div>
    `
}