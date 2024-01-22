dotnet ef dbcontext scaffold "Host=localhost;Database=awesum;Username=postgres;Password=This4Now!" Npgsql.EntityFrameworkCore.PostgreSQL -o ./Model/ -f
Remove-Item "../awesum.client/src/serverClasses/*"
dotnet cs2ts ./Model/ -i Simple -o ../awesum.client/src/serverClasses/
dotnet cs2ts ./ControllerResponses/ -i Simple -o ../awesum.client/src/serverClasses/
rm ../awesum.client/src/serverClasses/awesumContext.ts
Get-ChildItem '../awesum.client/src/serverClasses/*.ts' -Recurse | ForEach {
    (Get-Content $_) -replace 'export interface ', 'export interface Server' | Set-Content $_
     (Get-Content $_) -replace ' } from', ' } from' | Set-Content $_
     (Get-Content $_) -replace 'import { ', 'import type { Server' | Set-Content $_
     (Get-Content $_) -replace ': Queryable<', ': Array<' | Set-Content $_
     (Get-Content $_) -replace 'import type { ServerQueryable } from "./queryable";', '' | Set-Content $_
     (Get-Content $_) -replace '<', '<Server' | Set-Content $_
     (Get-Content $_) -replace 'apps: App\[\];', '' | Set-Content $_
     (Get-Content $_) -replace 'string \| null;', 'string;' | Set-Content $_
     (Get-Content $_) -replace 'number \| null;', 'number;' | Set-Content $_
     (Get-Content $_) -replace 'boolean \| null;', 'boolean;' | Set-Content $_
    (Get-Content $_) -replace 'import type { (.*) } from "./(.*)"', 'import type { $1 } from "./$1"' | Set-Content $_ 


}

Get-Item -Path "../awesum.client/src/serverClasses/*" | Rename-Item -NewName { "Server" + $_.Name.Substring(0,1).ToUpper() + $_.Name.Substring(1).Replace(".Ts",".ts")}
Copy-Item -Path "../awesum.client/src/serverClasses/*" -Destination "../awesum.client/src/clientClasses/" -Recurse
Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | where-object {$_.Name -notlike '*Response.ts'} | ForEach {(Get-Content $_| Out-String).Trim() -replace "    id: number;", "" | Set-Content $_}
Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | where-object {$_.Name -notlike '*Response.ts'} | ForEach {(Get-Content $_| Out-String).Trim() -replace '    ([A-Za-z0-9]+?:)', '    private _$1' | Set-Content $_}
Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | where-object {$_.Name -notlike '*Response.ts'} | ForEach {(Get-Content $_| Out-String).Trim() -replace 'export interface ([A-Za-z0-9].*) {', 'import type { $1 as $1Interface } from "@/serverClasses/$1";
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
    ' | Set-Content $_}
    Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | where-object {$_.Name -notlike '*Response.ts'} | ForEach {(Get-Content $_| Out-String).Trim() -replace ": string;", ": string = '';" | Set-Content $_}
    Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | where-object {$_.Name -notlike '*Response.ts'} | ForEach {(Get-Content $_| Out-String).Trim() -replace ": number;", ": number = 0;" | Set-Content $_}
    Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | where-object {$_.Name -notlike '*Response.ts'} | ForEach {(Get-Content $_| Out-String).Trim() -replace ": boolean;", ": boolean = false;" | Set-Content $_}


Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | where-object {$_.Name -notlike '*Response.ts'} | ForEach {(Get-Content $_| Out-String).Trim() -replace '\n.*public .*', '' | Set-Content $_}
Get-ChildItem '../awesum.client/src/clientClasses/*.ts' -Recurse | where-object {$_.Name -notlike '*Response.ts'} | ForEach {(Get-Content $_) -replace '    private _(.*):(.*)(=.*);.*', '    private _$1:$2$3;
public get $1():$2 { return this._$1; }public set $1(v:$2) {this._$1=v;this.promises.push(Global.setTablePropertyValueById(this.id, ''$1'',v,this.table,this.promises))}' | Set-Content $_}
