import type {AuthorLookupDTO} from "../author/author-lookup-dto.ts";
import type {PublisherLookupDTO} from "../publisher/publisher-lookup-dto.ts";

export interface CreateTitleDTO {
    name: string;
    dateOfPublishing: string;
    isbn: number;
    genres: string[];
    authors: AuthorLookupDTO[];
    publisher: PublisherLookupDTO | null;
}