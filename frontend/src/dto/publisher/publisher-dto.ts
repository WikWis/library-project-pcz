import type {TitleLookupDTO} from "../title/title-lookup-dto.ts";

export interface PublisherDTO {
    PublisherId: number;
    Name: string;
    Titles: TitleLookupDTO[];
}