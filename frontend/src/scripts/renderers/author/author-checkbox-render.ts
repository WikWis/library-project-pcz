import type {AuthorLookupDTO} from "../../../dto/author/author-lookup-dto.ts";

function renderAuthorCheckbox(authorLookupDTO: AuthorLookupDTO): string {
    return `<div class="form-check">
                <input
                    type="checkbox"
                    name="authorId"
                    value="${authorLookupDTO.authorId}"
                    class="form-check-input"
                    id="author-${authorLookupDTO.authorId}"
                />
                <label class="form-check-label" for="author-${authorLookupDTO.authorId}">
                    ${authorLookupDTO.authorId} ${authorLookupDTO.name} ${authorLookupDTO.surname}
                </label>
            </div>
    `;
}

export function renderAuthorCheckboxes(authorLookupDTOS: AuthorLookupDTO[]): string {
    return authorLookupDTOS.map(renderAuthorCheckbox).join("<br>")
}