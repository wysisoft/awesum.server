export interface Database {
    id: number;
    name: string;
    app: number;
    databaseid: string;
    version: string | null;
    appNavigation: App;
}