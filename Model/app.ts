export interface App {
    id: number;
    creator: number;
    appId: string;
    creatorNavigation: User;
    databases: Database[];
    followerApps: Follower[];
    followerFollowerapps: Follower[];
    followrequestApps: Followrequest[];
    followrequestFollowerapps: Followrequest[];
}