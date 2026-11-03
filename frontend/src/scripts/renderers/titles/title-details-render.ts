import type {TitleDTO} from "../../../dto/title/title-dto.ts";
import type {AuthorLookupDTO} from "../../../dto/author/author-lookup-dto.ts";
import type {PublisherLookupDTO} from "../../../dto/publisher/publisher-lookup-dto.ts";

function renderAuthors(authorLookupDTOS: AuthorLookupDTO[]): string {
    return authorLookupDTOS
        .map(author => `<li class="list-group-item">${author.name}</li>`)
        .join("\n")
}

function renderGenres(genres: string[]): string {
    return genres
        .map(genre => ` <span class="badge bg-secondary me-1">
                                    ${genre}
                                </span>`)
        .join("\n")
}

function renderPublisher(publisherLookupDTO: PublisherLookupDTO | null): string {
    if (publisherLookupDTO === null) {
        return `<span class="text-muted">
                    Brak przypisanego wydawcy
                </span>`
    }

    return `<span>
                ${publisherLookupDTO.name}
            </span>`
}

export function renderTitleDetails(titleDto: TitleDTO) {
    return `
        <div class="container mt-4">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0">Szczegóły tytułu</h4>
                    </div>
            
                    <div class="card-body">
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <strong>ID:</strong>
                                <span>${titleDto.titleId}</span>
                            </div>
                            <div class="col-md-6">
                                <strong>Nazwa:</strong>
                                <span>${titleDto.name}</span>
                            </div>
                        </div>
            
                        <div class="row mb-3">
                            <div class="col-md-6">
                                <strong>Data publikacji:</strong>
                                <span>${titleDto.dateOfPublishing}</span>
                            </div>
                            <div class="col-md-6">
                                <strong>ISBN:</strong>
                                <span>${titleDto.isbn}</span>
                            </div>
                        </div>
            
                        <hr>
            
                        <div class="mb-3">
                            <strong>Gatunki:</strong>
                            <div class="mt-2">
                               ${renderGenres(titleDto.genres)}
                            </div>
                        </div>
            
                        <div class="mb-3">
                            <strong>Autorzy:</strong>
                            <ul class="list-group mt-2">
                                ${renderAuthors(titleDto.authors)}
                            </ul>
                        </div>
            
                        <div class="mb-3">
                            <strong>Wydawca:</strong>
                            <div class="mt-2">
                                ${renderPublisher(titleDto.publisher)}
                            </div>
                        </div>
                    </div>
                </div>
        </div>   
    `
}