import {DetailsSubject} from "../details/details-subject.ts";
import type {PublisherDTO} from "../../../dto/publisher/publisher-dto.ts";
import {PublisherService, type PublisherServiceInterface} from "../../../services/publisher-service.ts";

export class PublisherDetailsSubject extends DetailsSubject<PublisherDTO> {

    private readonly publisherService: PublisherServiceInterface = new PublisherService();

    async getDetailsElement(id: number): Promise<PublisherDTO | undefined> {
        this._element = await this.publisherService.Details(id);

        this._gotElement = this._element !== undefined;
        this.notify();

        return this._element;
    }
}