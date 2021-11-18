using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Temp
{
    [Migration(9)]
    public class FillDictionaries : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"
                INSERT INTO item_types (id, name)
                VALUES 
                    (1, 'TShirt'),
                    (2, 'Sweatshirt'),
                    (3, 'Cup'),
                    (4, 'Notepad'),
                    (5, 'Bag'),
                    (6, 'Pen'),
                    (7, 'Socks')
                ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO pack_title (id, name)
                VALUES 
                    (10, 'WelcomePack'),
                    (20, 'StarterPack'),
                    (30, 'ConferenceListenerPack'),
                    (40, 'ConferenceSpeakerPack'),
                    (50, 'VeteranPack')
                ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO merch_request_status (id, name)
                VALUES 
                    (1, 'Draft'),
                    (2, 'Created'),
                    (3, 'Assigned'),
                    (4, 'InProgress'),
                    (5, 'Done')
                ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO clothing_sizes (id, name)
                VALUES 
                    (1, 'XS'),
                    (2, 'S'),
                    (3, 'M'),
                    (4, 'L'),
                    (5, 'XL'),
                    (6, 'XXL')
                ON CONFLICT DO NOTHING
            ");
        }
    }
}