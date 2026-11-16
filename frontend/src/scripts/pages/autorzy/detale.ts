import {AuthorService, type AuthorServiceInterface} from "../../../services/author-service.ts";
import type {TitleLookupDTO} from "../../../dto/title/title-lookup-dto.ts";
import type {AuthorDTO} from "../../../dto/author/author-dto.ts";
import {PageSwitcher} from "../../pagination/page-switcher.ts";
import type {PageRendererInterface} from "../../pagination/page-renderer.ts";
import type {PageDataProviderInterface} from "../../pagination/page-data-provider/page-data-provider.ts";
import {TitleLookupPageDataProvider} from "../../pagination/page-data-provider/title-lookup-page-data-provider.ts";
import {setupPagination} from "../../pagination/pagination.ts";
import {renderTitlesGroupList} from "../../renderers/titles/title-group-list-render.ts";

// Getting query param
const params: URLSearchParams = new URLSearchParams(window.location.search);
const pathId: string | null = params.get("id");

// For Not Found Validation
const notFoundMessageDiv = document.querySelector<HTMLDivElement>("#not-found-message")
const contentDiv = document.querySelector<HTMLDivElement>("#content")

// Astro elements
const authorIdDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#author-id")
const authorNameDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#author-name")
const authorSurnameDiv: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#author-surname")
const authorTitlesList: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#author-titles-list");
const previousButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#previous-button")
const nextButton: HTMLButtonElement | null = document.querySelector<HTMLButtonElement>("#next-button")

const editButton: HTMLLinkElement | null = document.querySelector<HTMLLinkElement>("#edit-button")
const confirmRemoveButton: HTMLLinkElement | null = document.querySelector<HTMLLinkElement>("#confirmRemoveButton")

// When not created
const errorAlert: HTMLDivElement | null = document.querySelector<HTMLDivElement>("#author-danger-alert");

function setUpAstroElements(authorDto: AuthorDTO) {
    authorIdDiv!.innerText = authorDto.authorId.toString()
    authorNameDiv!.innerText = authorDto.name;
    authorSurnameDiv!.innerText = authorDto.surname;
    authorSurnameDiv!.innerText = authorDto.surname;
    editButton!.href = `/autorzy/edytuj?id=${authorDto.authorId.toString()}`
}

export async function init() {
    if (authorTitlesList === null || previousButton === null || nextButton === null || errorAlert === null) {
        throw new Error("No all elements needed are present.");
    }
    const authorService: AuthorServiceInterface = new AuthorService()

    const id: Number = Number.parseInt(pathId ?? "0")
    const authorDto: AuthorDTO | undefined = await new AuthorService().Details(id)

    if (id === 0 || isNaN(id.valueOf()) || authorDto === undefined || confirmRemoveButton === null) {
        notFoundMessageDiv!.hidden = false;
        contentDiv!.hidden = true;
        return;
    }

    const pageDataProvider: PageDataProviderInterface<TitleLookupDTO> = new TitleLookupPageDataProvider(authorDto.authorId)
    const pageRenderer: PageRendererInterface<TitleLookupDTO> = new class implements PageRendererInterface<TitleLookupDTO> {
        render(data: TitleLookupDTO[]): string {
            return renderTitlesGroupList(data);
        }
    }
    const pageSwitcher: PageSwitcher<TitleLookupDTO> = new PageSwitcher<TitleLookupDTO>(pageDataProvider, pageRenderer, authorTitlesList)

    setUpAstroElements(authorDto);
    await setupPagination(pageSwitcher, previousButton, nextButton);

    confirmRemoveButton.onclick = async () => {
        const isSaved: Boolean = await authorService.Delete(authorDto.authorId);
        if (!isSaved) {
            errorAlert.classList.toggle("d-none", isSaved);
            return;
        }

        window.location.href = "/autorzy";
    }
}