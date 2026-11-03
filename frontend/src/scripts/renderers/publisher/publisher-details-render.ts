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