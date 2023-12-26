export interface Follower {
    id: number;
    appid: number;
    followerappid: number;
    version: string | null;
    followeruser: number | null;
    app: App;
    followerapp: App;
}