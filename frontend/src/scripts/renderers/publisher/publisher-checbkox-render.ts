import type {PublisherLookupDTO} from "../../../dto/publisher/publisher-lookup-dto.ts";

function renderPublisherCheckbox(publisherLookupDTO: PublisherLookupDTO): string {
    return `<div class="form-check">
                <input
                    type="radio"
                    name="publisherId"
                    value="${publisherLookupDTO.publisherId}"
                    class="form-check-input"
                    id="publisher-${publisherLookupDTO.publisherId}"
                />
                <label class="form-check-label" for="author-${publisherLookupDTO.publisherId}">
                    ${publisherLookupDTO.publisherId} ${publisherLookupDTO.name}
                </label>
            </div>
    `;
}

export function renderPublisherCheckboxes(publisherLookupDTOS: PublisherLookupDTO[]): string {
    return publisherLookupDTOS.map(renderPublisherCheckbox).join("<br>")
}