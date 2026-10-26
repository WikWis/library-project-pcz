export interface PageNumberInterface {
    provideNext(): Number

    providePrevious(): Number

    lookUp(): Number
}

export class PageNumberProvider implements PageNumberInterface {

    private counter: Number = 0;

    public provideNext(): Number {
        this.counter = this.counter.valueOf() + 1;
        return this.counter
    }

    public providePrevious(): Number {
        this.counter = this.counter.valueOf() - 1;

        if (this.counter.valueOf() < 0) {
            this.counter = 0;
            return 0;
        }

        return this.counter
    }

    public lookUp() {
        return this.counter;
    }


}