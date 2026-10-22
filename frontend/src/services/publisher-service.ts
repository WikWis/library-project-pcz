import axios, {type AxiosRequestConfig} from "axios";
import {config} from "../config/config.ts";
import type {PublisherLookupDTO} from "../dto/publisher/publisher-lookup-dto.ts";
import type {PublisherDTO} from "../dto/publisher/publisher-dto.ts";
import type {CreatePublisherDTO} from "../dto/publisher/create-publisher-dto.ts";
import type {UpdatePublisherDTO} from "../dto/publisher/update-publisher-dto.ts";
import type {AuthorDTO} from "../dto/author/author-dto.ts";

export interface PublisherServiceInterface {
    Index(page?: Number, pageSize?: Number): Promise<PublisherLookupDTO[]>

    Details(publisherId: Number): Promise<PublisherDTO | undefined>

    Create(publisherDto: CreatePublisherDTO): Promise<PublisherDTO | undefined>


    Update(publisherId: Number, publisherDto: UpdatePublisherDTO): Promise<PublisherDTO | undefined>

    Delete(publisherId: Number): Promise<boolean>
}

export class PublisherService implements PublisherServiceInterface {

    private static endpoint: string = config.public.backendUrl + "/publishers"

    async Index(page?: Number, pageSize?: Number): Promise<PublisherLookupDTO[]> {
        const config: AxiosRequestConfig = {
            params: {
                page: page ?? null,
                pageSize: pageSize ?? null
            }
        }

        const response = await axios.get<PublisherLookupDTO[]>(PublisherService.endpoint, config);
        return response.data;
    }

    async Details(publisherId: Number): Promise<PublisherDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.get<PublisherDTO>(
            PublisherService.endpoint + `/${publisherId}`,
            config
        );

        if (response.status == 404) {
            return undefined;
        }

        return response.data;
    }

    async Create(publisherDTO: CreatePublisherDTO): Promise<PublisherDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500
        }

        const response = await axios.post<PublisherDTO>(
            PublisherService.endpoint,
            publisherDTO,
            config
        );

        if (response.status == 400) {
            return undefined;
        }

        return response.data;
    }

    async Update(publisherId: Number, publisherDto: UpdatePublisherDTO): Promise<PublisherDTO | undefined> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.put<PublisherDTO>(
            PublisherService.endpoint + `/${publisherId}`,
            publisherDto,
            config
        );

        if (response.status == 404 || response.status == 400) {
            return undefined;
        }

        return response.data;
    }

    async Delete(publisherId: Number): Promise<boolean> {
        const config: AxiosRequestConfig = {
            validateStatus: (status) => status >= 200 && status < 500,
        }

        const response = await axios.delete<AuthorDTO>(
            PublisherService.endpoint + `/${publisherId}`,
            config
        );

        if (response.status == 204) {
            return true;
        }

        if (response.status == 404 || response.status == 400) {
            return false;
        }

        return false
    }
}