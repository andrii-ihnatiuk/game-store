import { Configuration } from './configuration';

export const appConfiguration = new Configuration();

export async function resolveConfigurations(): Promise<void> {
  const routeConfig = '/assets/configuration/configuration.json';
  const response = await fetch(routeConfig);
  const data = await response.json();

  Object.keys(appConfiguration).forEach((x) => {
    if (!data[x]) {
      return;
    }

    (appConfiguration as any)[x] = data[x];
  });
}
