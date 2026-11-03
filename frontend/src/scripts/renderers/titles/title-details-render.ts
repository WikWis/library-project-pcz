import type {AuthorLookupDTO} from "../../../dto/author/author-lookup-dto.ts";
import type {PublisherLookupDTO} from "../../../dto/publisher/publisher-lookup-dto.ts";

export function renderAuthors(authorLookupDTOS: AuthorLookupDTO[]): string {
    return authorLookupDTOS
        .map(author => `<li class="list-group-item">${author.name}</li>`)
        .join("\n")
}

export function renderGenres(genres: string[]): string {
    return genres
        .map(genre => ` <span class="badge bg-secondary me-1">
                                    ${genre}
                                </span>`)
        .join("\n")
}

export function renderPublisher(publisherLookupDTO: PublisherLookupDTO | null): string {
    if (publisherLookupDTO === null) {
        return `<span class="text-muted">
                    Brak przypisanego wydawcy
                </span>`
    }

    return `<span>
                ${publisherLookupDTO.name}
            </span>`
}