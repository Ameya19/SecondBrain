using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecondBrain.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LowerCaseTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChunkTags_KnowledgeChunks_ChunkId",
                table: "ChunkTags");

            migrationBuilder.DropForeignKey(
                name: "FK_ChunkTags_Tags_TagId",
                table: "ChunkTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Contradictions_KnowledgeChunks_ChunkAId",
                table: "Contradictions");

            migrationBuilder.DropForeignKey(
                name: "FK_Contradictions_KnowledgeChunks_ChunkBId",
                table: "Contradictions");

            migrationBuilder.DropForeignKey(
                name: "FK_IngestionJob_Sources_SourceId",
                table: "IngestionJob");

            migrationBuilder.DropForeignKey(
                name: "FK_KnowledgeChunks_Sources_SourceId",
                table: "KnowledgeChunks");

            migrationBuilder.DropForeignKey(
                name: "FK_QueryChunkLinks_KnowledgeChunks_ChunkId",
                table: "QueryChunkLinks");

            migrationBuilder.DropForeignKey(
                name: "FK_QueryChunkLinks_Queries_QueryId",
                table: "QueryChunkLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sources",
                table: "Sources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QueryChunkLinks",
                table: "QueryChunkLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Queries",
                table: "Queries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KnowledgeChunks",
                table: "KnowledgeChunks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contradictions",
                table: "Contradictions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChunkTags",
                table: "ChunkTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngestionJob",
                table: "IngestionJob");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "tags");

            migrationBuilder.RenameTable(
                name: "Sources",
                newName: "sources");

            migrationBuilder.RenameTable(
                name: "QueryChunkLinks",
                newName: "querychunklinks");

            migrationBuilder.RenameTable(
                name: "Queries",
                newName: "queries");

            migrationBuilder.RenameTable(
                name: "KnowledgeChunks",
                newName: "knowledgechunks");

            migrationBuilder.RenameTable(
                name: "Contradictions",
                newName: "contradictions");

            migrationBuilder.RenameTable(
                name: "ChunkTags",
                newName: "chunktags");

            migrationBuilder.RenameTable(
                name: "IngestionJob",
                newName: "ingestionjobs");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "tags",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "tags",
                newName: "createdat");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tags",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "sources",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "sources",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "sources",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "sources",
                newName: "tags");

            migrationBuilder.RenameColumn(
                name: "PublishedAt",
                table: "sources",
                newName: "publishedat");

            migrationBuilder.RenameColumn(
                name: "IngestedAt",
                table: "sources",
                newName: "ingestedat");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sources",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RelevanceScore",
                table: "querychunklinks",
                newName: "relevancescore");

            migrationBuilder.RenameColumn(
                name: "ChunkId",
                table: "querychunklinks",
                newName: "chunkid");

            migrationBuilder.RenameColumn(
                name: "QueryId",
                table: "querychunklinks",
                newName: "queryid");

            migrationBuilder.RenameIndex(
                name: "IX_QueryChunkLinks_ChunkId",
                table: "querychunklinks",
                newName: "IX_querychunklinks_chunkid");

            migrationBuilder.RenameColumn(
                name: "Question",
                table: "queries",
                newName: "question");

            migrationBuilder.RenameColumn(
                name: "ModelUsed",
                table: "queries",
                newName: "modelused");

            migrationBuilder.RenameColumn(
                name: "AskedAt",
                table: "queries",
                newName: "askedat");

            migrationBuilder.RenameColumn(
                name: "Answer",
                table: "queries",
                newName: "answer");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "queries",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "knowledgechunks",
                newName: "sourceid");

            migrationBuilder.RenameColumn(
                name: "LastAccessedAt",
                table: "knowledgechunks",
                newName: "lastaccessedat");

            migrationBuilder.RenameColumn(
                name: "IngestedAt",
                table: "knowledgechunks",
                newName: "ingestedat");

            migrationBuilder.RenameColumn(
                name: "Embedding",
                table: "knowledgechunks",
                newName: "embedding");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "knowledgechunks",
                newName: "content");

            migrationBuilder.RenameColumn(
                name: "AccessCount",
                table: "knowledgechunks",
                newName: "accesscount");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "knowledgechunks",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_KnowledgeChunks_SourceId",
                table: "knowledgechunks",
                newName: "IX_knowledgechunks_sourceid");

            migrationBuilder.RenameIndex(
                name: "IX_KnowledgeChunks_Embedding",
                table: "knowledgechunks",
                newName: "IX_knowledgechunks_embedding");

            migrationBuilder.RenameColumn(
                name: "Resolved",
                table: "contradictions",
                newName: "resolved");

            migrationBuilder.RenameColumn(
                name: "ResolutionNote",
                table: "contradictions",
                newName: "resolutionnote");

            migrationBuilder.RenameColumn(
                name: "DetectedAt",
                table: "contradictions",
                newName: "detectedat");

            migrationBuilder.RenameColumn(
                name: "ChunkBId",
                table: "contradictions",
                newName: "chunkbid");

            migrationBuilder.RenameColumn(
                name: "ChunkAId",
                table: "contradictions",
                newName: "chunkaid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "contradictions",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Contradictions_ChunkBId",
                table: "contradictions",
                newName: "IX_contradictions_chunkbid");

            migrationBuilder.RenameIndex(
                name: "IX_Contradictions_ChunkAId",
                table: "contradictions",
                newName: "IX_contradictions_chunkaid");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "chunktags",
                newName: "tagid");

            migrationBuilder.RenameColumn(
                name: "ChunkId",
                table: "chunktags",
                newName: "chunkid");

            migrationBuilder.RenameIndex(
                name: "IX_ChunkTags_TagId",
                table: "chunktags",
                newName: "IX_chunktags_tagid");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "ingestionjobs",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "ingestionjobs",
                newName: "startedat");

            migrationBuilder.RenameColumn(
                name: "SourceId",
                table: "ingestionjobs",
                newName: "sourceid");

            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                table: "ingestionjobs",
                newName: "errormessage");

            migrationBuilder.RenameColumn(
                name: "CompletedAt",
                table: "ingestionjobs",
                newName: "completedat");

            migrationBuilder.RenameColumn(
                name: "ChunksCreated",
                table: "ingestionjobs",
                newName: "chunkscreated");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ingestionjobs",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_IngestionJob_SourceId",
                table: "ingestionjobs",
                newName: "IX_ingestionjobs_sourceid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tags",
                table: "tags",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sources",
                table: "sources",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_querychunklinks",
                table: "querychunklinks",
                columns: new[] { "queryid", "chunkid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_queries",
                table: "queries",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_knowledgechunks",
                table: "knowledgechunks",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contradictions",
                table: "contradictions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chunktags",
                table: "chunktags",
                columns: new[] { "chunkid", "tagid" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ingestionjobs",
                table: "ingestionjobs",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_chunktags_knowledgechunks_chunkid",
                table: "chunktags",
                column: "chunkid",
                principalTable: "knowledgechunks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chunktags_tags_tagid",
                table: "chunktags",
                column: "tagid",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_contradictions_knowledgechunks_chunkaid",
                table: "contradictions",
                column: "chunkaid",
                principalTable: "knowledgechunks",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_contradictions_knowledgechunks_chunkbid",
                table: "contradictions",
                column: "chunkbid",
                principalTable: "knowledgechunks",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_ingestionjobs_sources_sourceid",
                table: "ingestionjobs",
                column: "sourceid",
                principalTable: "sources",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_knowledgechunks_sources_sourceid",
                table: "knowledgechunks",
                column: "sourceid",
                principalTable: "sources",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_querychunklinks_knowledgechunks_chunkid",
                table: "querychunklinks",
                column: "chunkid",
                principalTable: "knowledgechunks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_querychunklinks_queries_queryid",
                table: "querychunklinks",
                column: "queryid",
                principalTable: "queries",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chunktags_knowledgechunks_chunkid",
                table: "chunktags");

            migrationBuilder.DropForeignKey(
                name: "fk_chunktags_tags_tagid",
                table: "chunktags");

            migrationBuilder.DropForeignKey(
                name: "fk_contradictions_knowledgechunks_chunkaid",
                table: "contradictions");

            migrationBuilder.DropForeignKey(
                name: "fk_contradictions_knowledgechunks_chunkbid",
                table: "contradictions");

            migrationBuilder.DropForeignKey(
                name: "fk_ingestionjobs_sources_sourceid",
                table: "ingestionjobs");

            migrationBuilder.DropForeignKey(
                name: "fk_knowledgechunks_sources_sourceid",
                table: "knowledgechunks");

            migrationBuilder.DropForeignKey(
                name: "fk_querychunklinks_knowledgechunks_chunkid",
                table: "querychunklinks");

            migrationBuilder.DropForeignKey(
                name: "fk_querychunklinks_queries_queryid",
                table: "querychunklinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tags",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sources",
                table: "sources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_querychunklinks",
                table: "querychunklinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_queries",
                table: "queries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_knowledgechunks",
                table: "knowledgechunks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_contradictions",
                table: "contradictions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chunktags",
                table: "chunktags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ingestionjobs",
                table: "ingestionjobs");

            migrationBuilder.RenameTable(
                name: "tags",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "sources",
                newName: "Sources");

            migrationBuilder.RenameTable(
                name: "querychunklinks",
                newName: "QueryChunkLinks");

            migrationBuilder.RenameTable(
                name: "queries",
                newName: "Queries");

            migrationBuilder.RenameTable(
                name: "knowledgechunks",
                newName: "KnowledgeChunks");

            migrationBuilder.RenameTable(
                name: "contradictions",
                newName: "Contradictions");

            migrationBuilder.RenameTable(
                name: "chunktags",
                newName: "ChunkTags");

            migrationBuilder.RenameTable(
                name: "ingestionjobs",
                newName: "IngestionJob");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Tags",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "createdat",
                table: "Tags",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tags",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "Sources",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "Sources",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Sources",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "tags",
                table: "Sources",
                newName: "Tags");

            migrationBuilder.RenameColumn(
                name: "publishedat",
                table: "Sources",
                newName: "PublishedAt");

            migrationBuilder.RenameColumn(
                name: "ingestedat",
                table: "Sources",
                newName: "IngestedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Sources",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "relevancescore",
                table: "QueryChunkLinks",
                newName: "RelevanceScore");

            migrationBuilder.RenameColumn(
                name: "chunkid",
                table: "QueryChunkLinks",
                newName: "ChunkId");

            migrationBuilder.RenameColumn(
                name: "queryid",
                table: "QueryChunkLinks",
                newName: "QueryId");

            migrationBuilder.RenameIndex(
                name: "IX_querychunklinks_chunkid",
                table: "QueryChunkLinks",
                newName: "IX_QueryChunkLinks_ChunkId");

            migrationBuilder.RenameColumn(
                name: "question",
                table: "Queries",
                newName: "Question");

            migrationBuilder.RenameColumn(
                name: "modelused",
                table: "Queries",
                newName: "ModelUsed");

            migrationBuilder.RenameColumn(
                name: "askedat",
                table: "Queries",
                newName: "AskedAt");

            migrationBuilder.RenameColumn(
                name: "answer",
                table: "Queries",
                newName: "Answer");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Queries",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "sourceid",
                table: "KnowledgeChunks",
                newName: "SourceId");

            migrationBuilder.RenameColumn(
                name: "lastaccessedat",
                table: "KnowledgeChunks",
                newName: "LastAccessedAt");

            migrationBuilder.RenameColumn(
                name: "ingestedat",
                table: "KnowledgeChunks",
                newName: "IngestedAt");

            migrationBuilder.RenameColumn(
                name: "embedding",
                table: "KnowledgeChunks",
                newName: "Embedding");

            migrationBuilder.RenameColumn(
                name: "content",
                table: "KnowledgeChunks",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "accesscount",
                table: "KnowledgeChunks",
                newName: "AccessCount");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "KnowledgeChunks",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_knowledgechunks_sourceid",
                table: "KnowledgeChunks",
                newName: "IX_KnowledgeChunks_SourceId");

            migrationBuilder.RenameIndex(
                name: "IX_knowledgechunks_embedding",
                table: "KnowledgeChunks",
                newName: "IX_KnowledgeChunks_Embedding");

            migrationBuilder.RenameColumn(
                name: "resolved",
                table: "Contradictions",
                newName: "Resolved");

            migrationBuilder.RenameColumn(
                name: "resolutionnote",
                table: "Contradictions",
                newName: "ResolutionNote");

            migrationBuilder.RenameColumn(
                name: "detectedat",
                table: "Contradictions",
                newName: "DetectedAt");

            migrationBuilder.RenameColumn(
                name: "chunkbid",
                table: "Contradictions",
                newName: "ChunkBId");

            migrationBuilder.RenameColumn(
                name: "chunkaid",
                table: "Contradictions",
                newName: "ChunkAId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Contradictions",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_contradictions_chunkbid",
                table: "Contradictions",
                newName: "IX_Contradictions_ChunkBId");

            migrationBuilder.RenameIndex(
                name: "IX_contradictions_chunkaid",
                table: "Contradictions",
                newName: "IX_Contradictions_ChunkAId");

            migrationBuilder.RenameColumn(
                name: "tagid",
                table: "ChunkTags",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "chunkid",
                table: "ChunkTags",
                newName: "ChunkId");

            migrationBuilder.RenameIndex(
                name: "IX_chunktags_tagid",
                table: "ChunkTags",
                newName: "IX_ChunkTags_TagId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "IngestionJob",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "startedat",
                table: "IngestionJob",
                newName: "StartedAt");

            migrationBuilder.RenameColumn(
                name: "sourceid",
                table: "IngestionJob",
                newName: "SourceId");

            migrationBuilder.RenameColumn(
                name: "errormessage",
                table: "IngestionJob",
                newName: "ErrorMessage");

            migrationBuilder.RenameColumn(
                name: "completedat",
                table: "IngestionJob",
                newName: "CompletedAt");

            migrationBuilder.RenameColumn(
                name: "chunkscreated",
                table: "IngestionJob",
                newName: "ChunksCreated");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "IngestionJob",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_ingestionjobs_sourceid",
                table: "IngestionJob",
                newName: "IX_IngestionJob_SourceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sources",
                table: "Sources",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QueryChunkLinks",
                table: "QueryChunkLinks",
                columns: new[] { "QueryId", "ChunkId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Queries",
                table: "Queries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KnowledgeChunks",
                table: "KnowledgeChunks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contradictions",
                table: "Contradictions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChunkTags",
                table: "ChunkTags",
                columns: new[] { "ChunkId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngestionJob",
                table: "IngestionJob",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChunkTags_KnowledgeChunks_ChunkId",
                table: "ChunkTags",
                column: "ChunkId",
                principalTable: "KnowledgeChunks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChunkTags_Tags_TagId",
                table: "ChunkTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contradictions_KnowledgeChunks_ChunkAId",
                table: "Contradictions",
                column: "ChunkAId",
                principalTable: "KnowledgeChunks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contradictions_KnowledgeChunks_ChunkBId",
                table: "Contradictions",
                column: "ChunkBId",
                principalTable: "KnowledgeChunks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IngestionJob_Sources_SourceId",
                table: "IngestionJob",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KnowledgeChunks_Sources_SourceId",
                table: "KnowledgeChunks",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QueryChunkLinks_KnowledgeChunks_ChunkId",
                table: "QueryChunkLinks",
                column: "ChunkId",
                principalTable: "KnowledgeChunks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QueryChunkLinks_Queries_QueryId",
                table: "QueryChunkLinks",
                column: "QueryId",
                principalTable: "Queries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
