import type { PublisherDTO } from "../../../dto/publisher/publisher-dto.ts";
import type { TitleLookupDTO } from "../../../dto/title/title-lookup-dto.ts";

function renderTitle(title: TitleLookupDTO): string {
    return `
        <li class="list-group-item d-flex justify-content-between align-items-center py-3">
            <div>
                <strong>${title.name}</strong>
                <br>
                <small class="text-muted">
                    ID: ${title.titleId}
                </small>
            </div>

            <span class="badge bg-secondary">
                ${title.dateOfPublishing}
            </span>
        </li>
    `
}

export function renderTitles(titles: TitleLookupDTO[]): string {
    if (titles.length === 0) {
        return `
            <div class="alert alert-light border text-muted mb-0">
                Wydawca nie ma przypisanych żadnych tytułów.
            </div>
        `
    }

    return `
        <ul class="list-group">
            ${titles.map(renderTitle).join("")}
        </ul>
    `
}

export function renderPublisherDetails(publisher: PublisherDTO): string {
    return `
        <div class="container-fluid mt-4 px-4">
            <div class="card shadow-sm w-100">

                <div class="card-header bg-primary text-white">
                    <h4 class="mb-0">Szczegóły wydawcy</h4>
                </div>

                <div class="card-body p-4">

                    <div class="row mb-3">
                        <div class="col-md-6">
                            <strong>ID wydawcy:</strong>
                            <span>${publisher.publisherId}</span>
                        </div>

                        <div class="col-md-6">
                            <strong>Nazwa:</strong>
                            <span>${publisher.name}</span>
                        </div>
                    </div>

                    <hr>

                    <div class="mb-3">
                        <strong>Opublikowane tytuły:</strong>

                        <div id="titles-list" class="mt-3"></div>
                    </div>

                    <div class="d-flex justify-content-between mt-3">
                        <button
                            type="button"
                            class="btn btn-primary"
                            id="previous-button"
                            disabled>
                            Previous Page
                        </button>

                        <button
                            type="button"
                            class="btn btn-primary"
                            id="next-button">
                            Next Page
                        </button>
                    </div>

                </div>
            </div>
        </div>
    `
}