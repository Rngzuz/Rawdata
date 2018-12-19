import { observable } from 'knockout'

class Prompt {
    constructor(params = {}) {
        const { onAccept, onCancel, showPrompt } = params

        this.onAccept = onAccept
        this.onCancel = onCancel
        this.showPrompt = showPrompt

        this.noteValue = observable('')

        const hideOnEscape = event => {
            if (event.keyCode === 27) {
                showPrompt(false)
            }
        }

        document.addEventListener('keyup', hideOnEscape)

        this.dispose = function () {
            document.removeEventListener('keyup', hideOnEscape)
        }
    }
}

const template = /* html */ `
<div class="modal" data-bind="css: { 'd-block show': showPrompt }">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Add note</h5>
                <button type="button" class="close" data-bind="click: () => { showPrompt(false) }">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <textarea class="form-control" placeholder="Enter an optional note here." rows="3" data-bind="value: noteValue"></textarea>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-bind="click: () => { onAccept(noteValue()); noteValue('') }">Accept</button>
                <button type="button" class="btn btn-secondary" data-bind="click: () => onCancel()">Cancel</button>
            </div>
        </div>
    </div>
    <div class="modal-backdrop" style="z-index: -1" data-bind="click: () => { showPrompt(false) }, css: { 'd-block show': showPrompt }"></div>
</div>
`

export default {
    viewModel: {
        createViewModel: (params, componentInfo) => new Prompt(params, componentInfo)
    },
    template
}