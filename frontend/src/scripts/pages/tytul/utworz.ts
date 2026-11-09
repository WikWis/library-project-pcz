import type {CreateTitleDTO} from "../../../dto/title/create-title-dto.ts";
import type {AuthorLookupDTO} from "../../../dto/author/author-lookup-dto.ts";
import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import {PublisherService, type PublisherServiceInterface} from "../../../services/publisher-service.ts";
import type {PublisherLookupDTO} from "../../../dto/publisher/publisher-lookup-dto.ts";
import {renderAuthorCheckboxes} from "../../renderers/author/author-checkbox-render.ts";
import {renderPublisherCheckboxes} from "../../renderers/publisher/publisher-checbkox-render.ts";
import {InputValidSubject} from "../../alert/input/input-valid-subject.ts";
import {InputValidObserver} from "../../alert/input/input-valid-observer.ts";
import {TitleSaveSubject} from "../../alert/title/title-save-subject.ts";
import {SaveObserver} from "../../alert/save/save-observer.ts";
import type {SaveSubject} from "../../alert/save/save-subject.ts";


const titleNameInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#title-name-input');
const titleDateOfPublishingInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#title-date-of-publishing-input');
const titleISBNInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#title-isbn-input');


const authorsCheckboxList: HTMLDivElement | null = document.querySelector<HTMLDivElement>('#authors-checkbox-list');
const publisherCheckboxList: HTMLDivElement | null = document.querySelector<HTMLDivElement>('#publisher-checkbox-list');


const createTitleButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>('#submit-title-button');
const creationSuccessAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#title-success-alert");
const errorAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#title-danger-alert");


function createTitleDTO(
    titleNameInput: HTMLInputElement,
    titleDateOfPublishingInput: HTMLInputElement,
    titleISBNInput: HTMLInputElement,
    titleSelectedGenres: HTMLInputElement[],
    titleSelectedAuthors: HTMLInputElement[],
    titleSelectedPublisher: HTMLInputElement | null
): CreateTitleDTO {

    const publisher = titleSelectedPublisher ?
        {
            publisherId: Number.parseInt(titleSelectedPublisher.value),
            name: "",
        } : null;

    return {
        name: titleNameInput.value.trim(),
        dateOfPublishing: titleDateOfPublishingInput.valueAsDate!.toISOString().slice(0, 10),
        isbn: Number.parseInt(titleISBNInput.value),
        genres: titleSelectedGenres.map(genre => genre.value),
        authors: titleSelectedAuthors.map(author => {
            return {
                authorId: Number.parseInt(author.value),
                name: "",
                surname: "",
            } as AuthorLookupDTO
        }),
        publisher: publisher,
    } as CreateTitleDTO;
}


async function getAllAuthors(pageSize = 100): Promise<AuthorLookupDTO[]> {
    const authorService: AuthorServiceInterface = new AuthorService();

    const authors: AuthorLookupDTO[] = [];

    let page: Number = 0;
    let result: AuthorLookupDTO[];
    do {
        result = await authorService.Index(undefined, page, pageSize);
        authors.push(...result);
        page = page.valueOf() + 1;
    } while (result.length != 0);

    return authors;
}

async function getAllPublishers(pageSize = 100): Promise<PublisherLookupDTO[]> {
    const publisherService: PublisherServiceInterface = new PublisherService();

    const publishers: PublisherLookupDTO[] = [];

    let page: Number = 0;
    let result: PublisherLookupDTO[];
    do {
        result = await publisherService.Index(page, pageSize);
        publishers.push(...result);
        page = page.valueOf() + 1;
    } while (result.length != 0);

    return publishers;
}

export async function init(): Promise<void> {
    if (!titleNameInput || !titleDateOfPublishingInput || !titleISBNInput || !createTitleButton || !creationSuccessAlert || !errorAlert) {
        throw new Error("Not every element is present!");
    }

    authorsCheckboxList!.innerHTML = renderAuthorCheckboxes(await getAllAuthors());
    publisherCheckboxList!.innerHTML = renderPublisherCheckboxes(await getAllPublishers());

    const titleCreationSubject: SaveSubject<CreateTitleDTO> = new TitleSaveSubject()
    const titleCreationObserver: SaveObserver = new SaveObserver((wasTitleCreated: Boolean) => {
        if (wasTitleCreated) {
            titleNameInput.value = "";
            titleDateOfPublishingInput.value = "";
            titleISBNInput.value = "";
        }
        creationSuccessAlert.classList.toggle("d-none", !wasTitleCreated);
        errorAlert.classList.toggle("d-none", wasTitleCreated.valueOf());
    });
    titleCreationSubject.attach(titleCreationObserver)

    const inputValidSubject: InputValidSubject = new InputValidSubject();
    const inputValidationObserver: InputValidObserver = new InputValidObserver((areInputsValid: Boolean) => {
        if (!areInputsValid) {
            errorAlert.classList.toggle("d-none", areInputsValid);
        }
    });
    inputValidSubject.attach(inputValidationObserver);

    createTitleButton.onclick = async () => {

        // Get checkbox inputs
        const titleSelectedGenres: HTMLInputElement[] = Array.from(
            document.querySelectorAll<HTMLInputElement>('input[id^="title-genre-input-"]:checked')
        )
        const titleSelectedAuthors: HTMLInputElement[] = Array.from(
            document.querySelectorAll<HTMLInputElement>('input[name="authorId"]:checked')
        )
        const titleSelectedPublisher: HTMLInputElement | null = document.querySelector<HTMLInputElement>('input[name="publisherId"]:checked')

        // Validate Inputs
        const areInputsValid: boolean = inputValidSubject.areInputsValid([
            titleNameInput,
            titleDateOfPublishingInput,
            titleISBNInput,
        ]);
        if (!areInputsValid) {
            return;
        }

        // Save Title
        const titleDto: CreateTitleDTO = createTitleDTO(
            titleNameInput,
            titleDateOfPublishingInput,
            titleISBNInput,
            titleSelectedGenres,
            titleSelectedAuthors,
            titleSelectedPublisher
        );
        await titleCreationSubject.saveElement(titleDto);
    }
}