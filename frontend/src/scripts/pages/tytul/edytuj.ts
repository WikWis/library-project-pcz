import type {TitleDTO} from "../../../dto/title/title-dto.ts";
import type {UpdateTitleDTO} from "../../../dto/title/update-title-dto.ts";
import type {AuthorLookupDTO} from "../../../dto/author/author-lookup-dto.ts";
import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import {PublisherService, type PublisherServiceInterface} from "../../../services/publisher-service.ts";
import type {PublisherLookupDTO} from "../../../dto/publisher/publisher-lookup-dto.ts";
import {renderAuthorCheckboxes} from "../../renderers/author/author-checkbox-render.ts";
import {renderPublisherCheckboxes} from "../../renderers/publisher/publisher-checbkox-render.ts";
import {InputValidSubject} from "../../alert/input/input-valid-subject.ts";
import {InputValidObserver} from "../../alert/input/input-valid-observer.ts";
import {TitleUpdateSubject} from "../../alert/title/title-update-subject.ts";
import {TitleDetailsSubject} from "../../alert/title/title-details-subject.ts";
import {SaveObserver} from "../../alert/save/save-observer.ts";
import {DetailsObserver} from "../../alert/details/details-observer.ts";


const params: URLSearchParams = new URLSearchParams(window.location.search);
const pathId: string | null = params.get("id");

const titleNameInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#title-name-input');
const titleDateOfPublishingInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#title-date-of-publishing-input');
const titleISBNInput: HTMLInputElement | null = document.querySelector<HTMLInputElement>('#title-isbn-input');

const authorsCheckboxList: HTMLDivElement | null = document.querySelector<HTMLDivElement>('#authors-checkbox-list');
const publisherCheckboxList: HTMLDivElement | null = document.querySelector<HTMLDivElement>('#publisher-checkbox-list');

const submitTitleButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>('#submit-title-button');

const creationSuccessAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#title-success-alert");
const errorAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#title-danger-alert");

const notFoundMessageDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#not-found-message");
const contentDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#content");


function createTitleDTO(
    id: Number,
    titleNameInput: HTMLInputElement,
    titleDateOfPublishingInput: HTMLInputElement,
    titleISBNInput: HTMLInputElement,
    titleSelectedGenres: HTMLInputElement[],
    titleSelectedAuthors: HTMLInputElement[],
    titleSelectedPublisher: HTMLInputElement | null
): UpdateTitleDTO {

    const publisher = titleSelectedPublisher ?
        {
            publisherId: Number.parseInt(titleSelectedPublisher.value),
            name: "",
        } : null;

    return {
        titleId: id,
        name: titleNameInput.value.trim(),
        dateOfPublishing: titleDateOfPublishingInput.valueAsDate!.toISOString().slice(0, 10),
        isbn: Number.parseInt(titleISBNInput.value),
        genres: titleSelectedGenres.map(genre => genre.value),
        authors: titleSelectedAuthors.map(author => {
            return {
                authorId: Number.parseInt(author.value),
                name: "",
                surname: "",
            } as AuthorLookupDTO;
        }),
        publisher: publisher,
    } as UpdateTitleDTO;
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

function fillTitleInputs(titleDto: TitleDTO): void {
    // Filling basic inputs
    console.log("d")
    titleNameInput!.value = titleDto.name;
    titleDateOfPublishingInput!.value = titleDto.dateOfPublishing.toString().slice(0, 10);
    titleISBNInput!.value = titleDto.isbn.toString();

    // Selecting genres
    const selectedGenres = new Set(titleDto.genres);

    const genreCheckboxes = Array.from(
        document.querySelectorAll<HTMLInputElement>('input[id^="title-genre-input-"]')
    );

    genreCheckboxes.forEach(checkbox => {
        checkbox.checked = selectedGenres.has(checkbox.value);
    });

    // Selecting authors
    const selectedAuthors = new Set(
        titleDto.authors.map(author => author.authorId)
    );
    console.log(selectedAuthors)

    const authorCheckboxes: HTMLInputElement[] = Array.from(
        document.querySelectorAll<HTMLInputElement>('input[name="authorId"]')
    );

    authorCheckboxes.forEach(checkbox => {
        checkbox.checked = selectedAuthors.has(Number.parseInt(checkbox.value));
        console.log(checkbox)
    });

    // Selecting publisher
    const publisherCheckboxes = Array.from(
        document.querySelectorAll<HTMLInputElement>('input[name="publisherId"]')
    );

    publisherCheckboxes.forEach(checkbox => {
        checkbox.checked = titleDto.publisher !== null && Number(checkbox.value) === titleDto.publisher.publisherId;
    });
}

export async function init(): Promise<void> {

    if (!titleNameInput || !titleDateOfPublishingInput || !titleISBNInput || !submitTitleButton || !creationSuccessAlert || !errorAlert || !notFoundMessageDiv || !contentDiv) {
        throw new Error("Not every element is present!");
    }

    // Getting title ID
    const id: Number = Number.parseInt(pathId ?? "0");
    if (id === 0 || isNaN(id.valueOf())) {
        notFoundMessageDiv.hidden = false;
        contentDiv.hidden = true;
        return;
    }

    // Getting authors and publishers
    authorsCheckboxList!.innerHTML = renderAuthorCheckboxes(await getAllAuthors());
    publisherCheckboxList!.innerHTML = renderPublisherCheckboxes(await getAllPublishers());

    // Getting title details
    const titleDetailsSubject: TitleDetailsSubject = new TitleDetailsSubject();
    const titleDetailsObserver = new DetailsObserver<TitleDTO | undefined>((titleDto) => {
            if (titleDto === undefined) {
                notFoundMessageDiv.hidden = false;
                contentDiv.hidden = true;
                return;
            }

            fillTitleInputs(titleDto);

            notFoundMessageDiv.hidden = true;
            contentDiv.hidden = false;
        }
    );
    titleDetailsSubject.attach(titleDetailsObserver);
    const titleDto: TitleDTO | undefined = await titleDetailsSubject.getDetailsElement(id);
    if (titleDto === undefined) {
        return;
    }

    // Showing content
    notFoundMessageDiv.hidden = true;
    contentDiv.hidden = false;

    // Preparing title update
    const titleUpdateSubject: TitleUpdateSubject = new TitleUpdateSubject();
    const titleUpdateObserver: SaveObserver = new SaveObserver((wasTitleUpdated: Boolean) => {
        creationSuccessAlert.classList.toggle("d-none", !wasTitleUpdated);
        errorAlert.classList.toggle("d-none", wasTitleUpdated.valueOf());

    });
    titleUpdateSubject.attach(titleUpdateObserver);

    // Input validation
    const inputValidSubject: InputValidSubject = new InputValidSubject();
    const inputValidationObserver: InputValidObserver = new InputValidObserver((areInputsValid: Boolean) => {
        if (!areInputsValid) {
            errorAlert.classList.toggle("d-none", areInputsValid);
        }
    });
    inputValidSubject.attach(inputValidationObserver);

    // Submit title
    submitTitleButton.onclick = async () => {
        // Get checkbox inputs
        const titleSelectedGenres: HTMLInputElement[] = Array.from(document.querySelectorAll<HTMLInputElement>('input[id^="title-genre-input-"]:checked'));
        const titleSelectedAuthors: HTMLInputElement[] = Array.from(document.querySelectorAll<HTMLInputElement>('input[name="authorId"]:checked'));
        const titleSelectedPublisher: HTMLInputElement | null = document.querySelector<HTMLInputElement>('input[name="publisherId"]:checked');

        // Validate inputs
        const areInputsValid: boolean = inputValidSubject.areInputsValid([
            titleNameInput,
            titleDateOfPublishingInput,
            titleISBNInput,
        ]);
        if (!areInputsValid) {
            return;
        }

        // Update Title
        const titleDto: UpdateTitleDTO = createTitleDTO(
            id,
            titleNameInput,
            titleDateOfPublishingInput,
            titleISBNInput,
            titleSelectedGenres,
            titleSelectedAuthors,
            titleSelectedPublisher
        );
        await titleUpdateSubject.saveElement(titleDto);
    };

}