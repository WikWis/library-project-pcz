import type {TitleLookupDTO} from "../title/title-lookup-dto.ts";

export interface PublisherDTO {
    publisherId: number;
    name: string;
    titles: TitleLookupDTO[];
}