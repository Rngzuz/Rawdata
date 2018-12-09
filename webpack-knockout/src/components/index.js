import { PageComponent } from './page/page.component.js'
import { default as PageTemplate } from './page/page.component.html'

import { TextComponent } from './text/text.component.js'
import { default as TextTemplate } from './text/text.component.html'

import { TestComponent } from './test/test.component.js'
import { default as TestTemplate } from './test/test.component.html'

export default [
    { name: 'page-component', viewModel: PageComponent, template: PageTemplate },
    { name: 'text-component', viewModel: TextComponent, template: TextTemplate },
    { name: 'test-component', viewModel: TestComponent, template: TestTemplate }
]