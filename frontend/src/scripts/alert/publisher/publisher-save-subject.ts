import {SaveSubject} from "../save/save-subject.ts";
import type {CreatePublisherDTO} from "../../../dto/publisher/create-publisher-dto.ts";
import {PublisherService, type PublisherServiceInterface} from "../../../services/publisher-service.ts";
import type {PublisherDTO} from "../../../dto/publisher/publisher-dto.ts";

export class PublisherSaveSubject extends SaveSubject<CreatePublisherDTO> {

    async saveElement(element: CreatePublisherDTO): Promise<boolean> {
        const publisherService: PublisherServiceInterface = new PublisherService();
        const savedPublisher: PublisherDTO | undefined = await publisherService.Create(element);

        this._isCreated = savedPublisher !== undefined
        this.notify()

        return this._isCreated;
    }

}