import type {PageSwitcher} from "./page-switcher.ts";

export async function setupPagination(
    pageSwitcher: PageSwitcher<any>,
    previousButton: HTMLButtonElement,
    nextButton: HTMLButtonElement
) {
    async function updateNavigationButtons() {
        previousButton.disabled = pageSwitcher.getCurrentPage().valueOf() === 0;
        nextButton.disabled = (await pageSwitcher.getPageItemCount()).valueOf() < 10;
    }

    previousButton.onclick = async () => {
        await pageSwitcher.previousPage();
        await updateNavigationButtons();
    };

    nextButton.onclick = async () => {
        await pageSwitcher.nextPage();
        await updateNavigationButtons();
    };

    await pageSwitcher.initialize();
    await updateNavigationButtons();
}