dotnet ef dbcontext scaffold "Name=ConnectionStrings:postgres" Npgsql.EntityFrameworkCore.PostgreSQL -o ./Model/ -f
Remove-Item "../awesum.client/src/serverClasses/*"
dotnet cs2ts ./Model/ -i Simple -o ../awesum.client/src/serverClasses/
dotnet cs2ts ./ControllerResponses/ -i Simple -o ../awesum.client/src/serverClasses/
dotnet cs2ts ./ControllerRequests/ -i Simple -o ../awesum.client/src/serverClasses/
rm ../awesum.client/src/serverClasses/awesumContext.ts
Get-ChildItem '../awesum.client/src/serverClasses/*.ts' -Recurse | ForEach-Object {
    (Get-Content $_) -replace 'export interface ', 'export interface Server' | Set-Content $_
     (Get-Content $_) -replace 'import { ', 'import type { Server' | Set-Content $_
     (Get-Content $_) -replace ': Queryable<', ': Array<' | Set-Content $_
     (Get-Content $_) -replace 'import type { ServerQueryable } from "./queryable";', '' | Set-Content $_
     (Get-Content $_) -replace '<', '<Server' | Set-Content $_
     (Get-Content $_) -replace 'apps: App\[\];', '' | Set-Content $_
     #(Get-Content $_) -replace 'string;', 'string | null;' | Set-Content $_
     #(Get-Content $_) -replace 'number;', 'number | null;' | Set-Content $_
     #(Get-Content $_) -replace 'boolean;', 'boolean | null;' | Set-Content $_
     (Get-Content $_) -replace 'App;', 'ServerApp;' | Set-Content $_
     (Get-Content $_) -replace 'DatabaseUnit;', 'ServerDatabaseUnit;' | Set-Content $_
     (Get-Content $_) -replace 'DatabaseItem;', 'ServerDatabaseItem;' | Set-Content $_
     (Get-Content $_) -replace 'DatabaseType;', 'ServerDatabaseType;' | Set-Content $_
     (Get-Content $_) -replace 'Database;', 'ServerDatabase;' | Set-Content $_
     (Get-Content $_) -replace 'Array<ServerFollower>;', 'Array<ServerFollower>;' | Set-Content $_


    (Get-Content $_) -replace 'import type { (.*) } from "./(.*)"', 'import type { $1 } from "./$1"' | Set-Content $_
    (Get-Content $_) -replace ': App;', ': ServerApp;' | Set-Content $_
    (Get-Content $_) -replace ': DatabaseUnit;', ': ServerDatabaseUnit;' | Set-Content $_
    (Get-Content $_) -replace ': DatabaseItem;', ': ServerDatabaseItem;' | Set-Content $_
    (Get-Content $_) -replace ': DatabaseType;', ': ServerDatabaseType;' | Set-Content $_

    (Get-Content $_) -replace ': DatabaseUnit\[\];', ': ServerDatabaseUnit[];' | Set-Content $_
    (Get-Content $_) -replace ': DatabaseItem\[\];', ': ServerDatabaseItem[];' | Set-Content $_
    (Get-Content $_) -replace ': DatabaseType\[\];', ': ServerDatabaseType[];' | Set-Content $_


}

Get-Item -Path "../awesum.client/src/serverClasses/*" | Rename-Item -NewName { "Server" + $_.Name.Substring(0, 1).ToUpper() + $_.Name.Substring(1).Replace(".Ts", ".ts") }
Copy-Item -Path "../awesum.client/src/serverClasses/*" -Destination "../awesum.client/src/clientClasses/" -Recurse
Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace "    id: number \| null;", "" | Set-Content $_ }
Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace '    ([A-Za-z0-9]+?:)', '    private _$1' | Set-Content $_ }
Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace 'export interface ([A-Za-z0-9].*) {', 'import type { $1 as $1Interface } from "@/serverClasses/$1";
import type { Table } from "dexie";
import { Global } from "@/global";

export class $1 implements $1Interface {
    constructor(other?:Partial<$1>|null, table?: Table) {
        if (other) {
            (this as any)["id"] = (other as any)["id"];
             for (var i in other) {
                  if (i == "id") continue;
                  (this as any)["_" + i] = (other as any)[i];
             }
        }
        if (table) {
             this.table = table;
        }
   }
   id = 0;
   table!: Table;
   promises = Array<Promise<void>>();
   async waitFor() {
        await Promise.all(this.promises);
        this.promises = Array<Promise<void>>();
   }
    ' | Set-Content $_ }



Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace "    private _id: number;", "" | Set-Content $_ }

Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace ": string;", ": string = '';" | Set-Content $_ }
Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace ": number;", ": number = 0;" | Set-Content $_ }
Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace ": boolean;", ": boolean = false;" | Set-Content $_ }

Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace '\n.*public .*', '' | Set-Content $_ }
Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | ForEach-Object { (Get-Content $_) -replace "    private _(.*): string = ''", '    private _$1: string = '''';
public get $1():string { return this._$1; }public set $1(v:string) {this._$1=v;this.promises.push(Global.setTablePropertyValueById(this.id, ''$1'',v,this.table,this.promises))}' | Set-Content $_ }

Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | ForEach-Object { (Get-Content $_) -replace "    private _(.*): number = 0", '    private _$1: number = 0;
public get $1():number { return this._$1; }public set $1(v:number) {this._$1=v;this.promises.push(Global.setTablePropertyValueById(this.id, ''$1'',v,this.table,this.promises))}' | Set-Content $_ }

Get-ChildItem './Model/*.cs' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace 'public string (.*) { get; set; } = null!;\n', 'public string $1 { get; set; } = "";
' | Set-Content $_ }
Get-ChildItem './Model/*.cs' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace 'public int (.*) { get; set; }\n', 'public int $1 { get; set; } = 0;
' | Set-Content $_ }
Get-ChildItem './Model/*.cs' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace 'public DateTime (.*) { get; set; }\n', 'public DateTime $1 { get; set; } = DateTime.Parse("1900-01-01");
' | Set-Content $_ }
Get-ChildItem './Model/*.cs' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace 'public bool (.*) { get; set; }\n', 'public bool $1 { get; set; } = false;
' | Set-Content $_ }
Get-ChildItem './Model/*.cs' -Recurse | ForEach-Object { (Get-Content $_ | Out-String).Trim() -replace 'public Guid (.*) { get; set; }\n', 'public Guid $1 { get; set; } = Guid.Empty;
' | Set-Content $_ }



