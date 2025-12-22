import type { Meta, StoryObj } from '@storybook/angular';
import { App } from './app';

const meta: Meta<App> = {
  title: 'Components/App',
  component: App,
  tags: ['autodocs'],
};

export default meta;
type Story = StoryObj<App>;

export const Default: Story = {};
